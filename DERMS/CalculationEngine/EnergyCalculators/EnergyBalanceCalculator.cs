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
using System;
using System.Threading;

namespace CalculationEngine.EnergyCalculators
{
    public class EnergyBalanceCalculator : ISubscriber, IEnergyBalanceCalculator
    {
        private Dictionary<DMSType, ITopologyCalculatingUnit> recalculatingUnits;

        private ITopologyCalculatingUnit energyProduction;
        private ITopologyCalculatingUnit energyConsumption;

        private ITopologyReader topologyReader;

        private IStorage<EnergySource> energySourceStorage;

        private Dictionary<long, EnergyBalanceCalculation> energyBalanceCalculations;

        private object locker;

        private EnergyBalanceStorage energyBalanceStorage;

        private Thread onCommitCalculationWorker;
        private CancellationTokenSource tokenSource;

        private AutoResetEvent topologyReadyEvent;

        public EnergyBalanceCalculator(EnergyBalanceStorage energyBalanceStorage, ITopologyAnalysis topologyAnalysisController)
        {
            this.energyBalanceStorage = energyBalanceStorage;
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

        public void PerformCalculation()
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

                    // TODO call commanding units, increase or decrease production or import less or more energy
                }
            }
        }

        private void LogCurrentEnergyBalance(EnergyBalanceCalculation calculation)
        {
            Logger.Instance.Log($"Source: {calculation.EnergySourceGid}");
            Logger.Instance.Log($"Demand: {calculation.Demand}");
            Logger.Instance.Log($"Response: {calculation.Production}\n");
        }

        public void Recalculate(long measurementGid, float newMeasurementValue)
        {
            lock (locker)
            {
                long conductingEquipmentGid = energyBalanceStorage.GetEntityByAnalogMeasurementGid(measurementGid);

                if (conductingEquipmentGid == 0)
                {
                    return;
                }             

                DMSType equipmentType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(measurementGid);

                if (equipmentType == DMSType.ENERGYSOURCE)
                {
                    energyBalanceCalculations[conductingEquipmentGid].Imported = newMeasurementValue;
                    return;
                }

                long sourceGid = topologyReader.FindSource(conductingEquipmentGid);

                EnergyBalanceCalculation calculation = energyBalanceCalculations[sourceGid];

                ITopologyCalculatingUnit calculatingUnit;
                if (!recalculatingUnits.TryGetValue(equipmentType, out calculatingUnit))
                {
                    Logger.Instance.Log($"[{GetType().Name}] Cannot find cooresponding energy calculating unit for dms type: {equipmentType}.");
                    return;
                }

                if (equipmentType == DMSType.ENERGYCONSUMER)
                {
                    calculation.Demand = calculatingUnit.Recalculate(calculation.Demand, conductingEquipmentGid, newMeasurementValue);
                }
                else
                {
                    calculation.Production = calculatingUnit.Recalculate(calculation.Production, conductingEquipmentGid, newMeasurementValue);
                }
            }

            // TODO call commanding units, increase or decrease production or import less or more energy
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(2)
            {
                new Subscription(Topic.AnalogRemotePointChange, new EnergyBalanceAnalogValueChanged(this)),
                new Subscription(Topic.DiscreteRemotePointChange, new EnergyBalanceTopologyChangedDynamicHandler(this))
            };
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
                energyBalanceStorage.Commited.WaitOne();
                topologyReadyEvent.WaitOne();

                if (cancellationToken.IsCancellationRequested)
                {
                    continue;
                }

                PerformCalculation();
            }
        }
    }
}
