using System;
using System.Collections.Generic;
using CalculationEngine.Commanding.Commands;
using CalculationEngine.DERStates.CommandScheduler.Commands;
using CalculationEngine.Commanding.DERCommanding.TimedCommandCreator.EnergyStorage.SchedulerCommandCreator;

namespace CalculationEngine.Commanding.DERCommanding.TimedCommandCreator.EnergyStorage
{
    public class EnergyStorageSchedulerCommandCreator : ISchedulerCommandCreator
    {
        private Dictionary<Type, ISchedulerCommandCreator> typeToCommandCreatorMap;

        public EnergyStorageSchedulerCommandCreator()
        {
            InitializeMap();
        }

        public SchedulerCommand CreateSchedulerCommand(Command command)
        {
            ISchedulerCommandCreator commandCreator;
            if (!typeToCommandCreatorMap.TryGetValue(command.GetType(), out commandCreator))
            {
                return null;
            }

            return commandCreator.CreateSchedulerCommand(command);
        }

        private void InitializeMap()
        {
            IdleStateCommandCreator idleCommandCreator = new IdleStateCommandCreator();

            typeToCommandCreatorMap = new Dictionary<Type, ISchedulerCommandCreator>()
            {
                { typeof(EnergyStorageIdleStateCommand), new RemoveCommandCreator() },
                { typeof(EnergyStorageDischargeCommand), idleCommandCreator },
                { typeof(EnergyStorageChargeCommand), idleCommandCreator },
            };
        }
    }
}
