using CalculationEngine.Model.DERStates;
using CalculationEngine.Model.EnergyCalculations;
using CalculationEngine.TopologyAnalysis;
using Common.ComponentStorage;
using Common.PubSub;
using System;
using System.Collections.Generic;
using System.Threading;
using Common.PubSub.Subscriptions;
using CalculationEngine.PubSub.DynamicHandlers;
using CalculationEngine.CommonComponents;
using Common.Helpers;
using Common.PubSub.Messages;
using Common.PubSub.Messages.DERState;
using CalculationEngine.TransactionProcessing.Storage.DERStates;
using Common.ServiceInterfaces.CalculationEngine;
using CalculationEngine.DERStates.CommandScheduler;
using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.DERStates
{
    /// <summary>
    /// Determines are DERs energized and publishes ActivePower of each DER.
    /// </summary>
    public class DERStateController : ITopologyDependentComponent, ISubscriber, IDisposable, IDERStateDeterminator
    {
        private ITopologyReader topologyReader;
        private DERStateStorage derStateStorage;
        private IStorage<EnergySource> energySourceStorage;

        private AdvancedSemaphore topologyReadyEvent;

        private object locker;

        private CancellationTokenSource cancellationTokenSource;
        private Thread determineDerStateWorker;

        private IDynamicPublisher dynamicPublisher;

        private ISchedulerCommandExecutor schedulerCommandExecutor;

        public DERStateController(IStorage<EnergySource> energySourceStorage, DERStateStorage derStateStorage, ITopologyAnalysis topologyAnalysis, IDynamicPublisher dynamicPublisher, ISchedulerCommandExecutor schedulerCommandExecutor)
        {
            this.derStateStorage = derStateStorage;
            this.dynamicPublisher = dynamicPublisher;
            this.energySourceStorage = energySourceStorage;
            this.schedulerCommandExecutor = schedulerCommandExecutor;

            topologyReadyEvent = topologyAnalysis.ReadyEvent;
            topologyReader = topologyAnalysis.CreateReader();

            locker = new object();

            cancellationTokenSource = new CancellationTokenSource();
            determineDerStateWorker = new Thread(() => OnCommitCalculation(cancellationTokenSource.Token));
            determineDerStateWorker.Start();
        }

        // Analog publication handling
        public void ProcessAnalogChanges(List<Tuple<long,float>> changes)
        {
            lock (locker)
            {
                DERStateChanged publication = new DERStateChanged();

                foreach (var analogChange in changes)
                {
                    long measurementGid = analogChange.Item1;
                    float measurementValue = analogChange.Item2;

                    derStateStorage.UpdateAnalogValue(measurementGid, measurementValue);

                    DERState derState = derStateStorage.GetEntityByAnalogMeasurementGid(measurementGid);
                    if (derState == null)
                    {
                        return;
                    }

                    if (!derState.IsEnergized)
                    {
                        return;
                    }

                    publication.DERStates.Add(new DERStateWrapper(derState.GlobalId, derState.ActivePower));

                }
                if (publication.DERStates.Count > 0)
                {
                    dynamicPublisher.Publish(publication);
                }
            }
        }

        //Discrete publication handling
        public void ProcessTopologyChanges()
        {
            lock (locker)
            {
                ICollection<long> energizedGids = null;
                List<EnergySource> energySources = energySourceStorage.GetAllEntities();
                foreach (var energySource in energySources)
                {
                    if (energizedGids == null)
                    {
                        energizedGids = topologyReader.Read(energySource.GlobalId);
                    }
                    else
                    {
                        foreach (long gid in topologyReader.Read(energySource.GlobalId))
                        {
                            energizedGids.Add(gid);
                        }
                    }      
                }

                List<DERState> derStates = derStateStorage.GetAllEntities();
                foreach (var entity in derStates)
                {
                    if (energizedGids.Contains(entity.GlobalId))
                    {
                        entity.IsEnergized = true;
                    }
                    else
                    {
                        entity.IsEnergized = false;
                    }

                    UpdateScheduledCommand(entity.GlobalId, entity.IsEnergized);
                }

                PublishDERStates();   
            }

        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
        }

        public IEnumerable<ISubscription> GetSubscriptions()
        {
            return new List<ISubscription>(2)
            {
                new Subscription(Topic.AnalogRemotePointChange, new CalculatingUnitAnalogValueChanged(this)),
                new Subscription(Topic.DiscreteRemotePointChange, new TopologyChangedDynamicHandler(this))
            };

        }

        public bool IsEntityEnergized(long entityGid)
        {
            var entity = derStateStorage.GetEntity(entityGid);
            if (entity == null)
            {
                return false;
            }

            return entity.IsEnergized;
        }

        private void OnCommitCalculation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                topologyReadyEvent.WaitOne();
                derStateStorage.Commited.WaitOne();

                if (cancellationToken.IsCancellationRequested)
                {
                    continue;
                }

                ProcessTopologyChanges();
            }
        }

        private void PublishDERStates()
        {
            DERStateChanged publication = new DERStateChanged();
            foreach (var derState in derStateStorage.GetAllEntities())
            {
                publication.DERStates.Add(new DERStateWrapper(derState.GlobalId, derState.ActivePower));
            }

            dynamicPublisher.Publish(publication);
        }

        private void UpdateScheduledCommand(long entityGid, bool shouldResumeCommand)
        {
            SchedulerCommand command;
            if (shouldResumeCommand)
            {
                command = new ResumeCommand(entityGid);
            }
            else
            {
                command = new PauseCommand(entityGid);
            }

            schedulerCommandExecutor.ExecuteCommand(command);
        }
    }
}
