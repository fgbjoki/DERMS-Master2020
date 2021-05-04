using CalculationEngine.EnergyCalculators.EnergyConsumption;
using CalculationEngine.EnergyCalculators.EnergyProduction;
using CalculationEngine.TopologyAnalysis;
using Common.PubSub;
using System.Collections.Generic;
using Common.PubSub.Subscriptions;
using CalculationEngine.TransactionProcessing.Storage.EnergyBalance;
using CalculationEngine.Model.EnergyCalculations;
using Common.ComponentStorage;
using CalculationEngine.PubSub.DynamicHandlers;
using Common.Logger;
using Common.AbstractModel;
using System.Threading;
using CalculationEngine.CommonComponents;
using Common.Helpers;
using Common.PubSub.Messages;
using System;

namespace CalculationEngine.EnergyCalculators
{
    public class EnergyBalanceCalculator : ISubscriber, ITopologyDependentComponent
    {
        private Dictionary<DMSType, ITopologyCalculatingUnit> recalculatingUnits;

        private ITopologyCalculatingUnit energyProduction;
        private ITopologyCalculatingUnit energyConsumption;

        private ITopologyReader topologyReader;

        private IDynamicPublisher dynamicPublisher;

        private IStorage<EnergySource> energySourceStorage;

        private Dictionary<long, EnergyBalanceCalculation> energyBalanceCalculations;

        private object locker;

        private EnergyBalanceStorage energyBalanceStorage;

        private Thread onCommitCalculationWorker;
        private CancellationTokenSource tokenSource;

        private AdvancedSemaphore topologyReadyEvent;

        public EnergyBalanceCalculator(EnergyBalanceStorage energyBalanceStorage, ITopologyAnalysis topologyAnalysisController, IDynamicPublisher dynamicPublisher)
        {
            this.energyBalanceStorage = energyBalanceStorage;
            this.dynamicPublisher = dynamicPublisher;

            topologyReadyEvent = topologyAnalysisController.ReadyEvent;

            energyBalanceCalculations = new Dictionary<long, EnergyBalanceCalculation>();

            energyProduction = new EnergyProductionCalculator(energyBalanceStorage.EnergyGeneratorStorage);
            energyConsumption = new EnergyConsumptionCalculator(energyBalanceStorage.EnergyConsumerStorage);
            energySourceStorage = energyBalanceStorage.EnergySourceStorage;

            topologyReader = topologyAnalysisController.CreateReader();

            locker = new object();

            InitializeRecalculatingUnits();

            tokenSource = new CancellationTokenSource();
            onCommitCalculationWorker = new Thread(() => OnCommitCalculation(tokenSource.Token));
            onCommitCalculationWorker.Start();
        }

        public void ProcessTopologyChanges()
        {
            lock (locker)
            {
                foreach (var energySource in energySourceStorage.GetAllEntities())
                {
                    EnergyBalanceCalculation calculation;
                    if (!energyBalanceCalculations.TryGetValue(energySource.GlobalId, out calculation))
                    {
                        calculation = new EnergyBalanceCalculation(energySource.GlobalId);
                        energyBalanceCalculations.Add(calculation.EnergySourceGid, calculation);
                    }

                    IEnumerable<long> connectedNodeGids = topologyReader.Read(energySource.GlobalId);

                    float demandValue = energyConsumption.Calculate(energySource.GlobalId, connectedNodeGids);
                    float producedValue = energyProduction.Calculate(energySource.GlobalId, connectedNodeGids);

                    calculation.Demand = demandValue;
                    calculation.Production = producedValue;

                    LogCurrentEnergyBalance(calculation);

                    EnergyBalanceChanged newEnergyBalance = new EnergyBalanceChanged()
                    {
                        DemandEnergy = calculation.Demand,
                        EnergySourceGid = calculation.EnergySourceGid,
                        ImportedEnergy = calculation.Imported,
                        ProducedEnergy = calculation.Production
                    };

                    dynamicPublisher.Publish(newEnergyBalance);
                }
            }
        }

        public void ProcessAnalogChanges(List<Tuple<long,float>> analogChanges)
        {
            lock (locker)
            {
                foreach (var analogChange in analogChanges)
                {
                    long conductingEquipmentGid = energyBalanceStorage.GetEntityByAnalogMeasurementGid(analogChange.Item1);

                    if (conductingEquipmentGid == 0)
                    {
                        return;
                    }

                    DMSType equipmentType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(conductingEquipmentGid);

                    if (equipmentType == DMSType.ENERGYSOURCE)
                    {
                        var currentCalculation = energyBalanceCalculations[conductingEquipmentGid];
                        currentCalculation.Imported = analogChange.Item2;
                        LogCurrentEnergyBalance(currentCalculation);

                        return;
                    }

                    float delta = ChangeAnalogValue(analogChange.Item1, analogChange.Item2);

                    long sourceGid = topologyReader.FindSource(conductingEquipmentGid);
                    if (sourceGid == 0)
                    {
                        // specified conducting equipment is not energized
                        return;
                    }

                    EnergyBalanceCalculation calculation = energyBalanceCalculations[sourceGid];

                    ITopologyCalculatingUnit calculatingUnit;
                    if (!recalculatingUnits.TryGetValue(equipmentType, out calculatingUnit))
                    {
                        Logger.Instance.Log($"[{GetType().Name}] Cannot find cooresponding energy calculating unit for dms type: {equipmentType}.");
                        return;
                    }

                    calculatingUnit.Recalculate(calculation, delta);

                    LogCurrentEnergyBalance(calculation);
                }

                foreach (var calculation in energyBalanceCalculations.Values)
                {
                    EnergyBalanceChanged newEnergyBalance = new EnergyBalanceChanged()
                    {
                        DemandEnergy = calculation.Demand,
                        EnergySourceGid = calculation.EnergySourceGid,
                        ImportedEnergy = calculation.Imported,
                        ProducedEnergy = calculation.Production
                    };

                    dynamicPublisher.Publish(newEnergyBalance);
                }       
            }

            // TODO call commanding units, increase or decrease production or import less or more energy
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(2)
            {
                new Subscription(Topic.AnalogRemotePointChange, new CalculatingUnitAnalogValueChanged(this)),
                new Subscription(Topic.DiscreteRemotePointChange, new TopologyChangedDynamicHandler(this))
            };
        }

        private void LogCurrentEnergyBalance(EnergyBalanceCalculation calculation)
        {
            Logger.Instance.Log($"Source: {calculation.EnergySourceGid}");
            Logger.Instance.Log($"Demand: {calculation.Demand}");
            Logger.Instance.Log($"Production: {calculation.Production}");
            Logger.Instance.Log($"Imported: {calculation.Imported}\n");
        }

        private float ChangeAnalogValue(long measurementGid, float newMeasurementValue)
        {
            float delta = 0;

            long conductingEquipmentGid =  energyBalanceStorage.GetEntityByAnalogMeasurementGid(measurementGid);

            CalculationObject calculationObject = energyBalanceStorage.GetEntity(conductingEquipmentGid);
            CalculationWrapper calculation = calculationObject.GetCalculation(measurementGid);

            if (calculation == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't update calculation of conducting equipment with gid: 0x{conductingEquipmentGid:X16}.");
                return delta;
            }

            delta = newMeasurementValue - calculation.Value;
            calculation.Value = newMeasurementValue;

            return delta;
        }

        private void InitializeRecalculatingUnits()
        {
            recalculatingUnits = new Dictionary<DMSType, ITopologyCalculatingUnit>(4)
            {
                { DMSType.ENERGYCONSUMER, energyConsumption },
                { DMSType.ENERGYSTORAGE, energyProduction },
                { DMSType.SOLARGENERATOR, energyProduction },
                { DMSType.WINDGENERATOR, energyProduction },
            };
        }

        private void OnCommitCalculation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                topologyReadyEvent.WaitOne();
                energyBalanceStorage.Commited.WaitOne();

                if (cancellationToken.IsCancellationRequested)
                {
                    continue;
                }

                ProcessTopologyChanges();
            }
        }
    }
}
