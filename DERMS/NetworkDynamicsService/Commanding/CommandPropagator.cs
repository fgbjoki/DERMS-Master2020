using Common.ComponentStorage;
using Common.Logger;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using NetworkDynamicsService.Commanding.CommandigProcessors;
using NetworkDynamicsService.Model.RemotePoints;
using System;
using System.Collections.Generic;

namespace NetworkDynamicsService.Commanding
{
    public class CommandPropagator : INDSCommanding
    {
        private Dictionary<Type, ICommandingProcessor> commandingProcessors;

        public CommandPropagator(IStorage<DiscreteRemotePoint> discreteStorage, IStorage<AnalogRemotePoint> analogStorage)
        {
            InitializeCommandingProcessors(discreteStorage, analogStorage);
        }

        public bool SendCommand(BaseCommand command)
        {
            ICommandingProcessor commandingProcessor;
            if (!commandingProcessors.TryGetValue(command.GetType(), out commandingProcessor))
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find command processor for command: {command.GetType()}!");
                return false;
            }

            return commandingProcessor.ProcessCommand(command);
        }

        private void InitializeCommandingProcessors(IStorage<DiscreteRemotePoint> discreteStorage, IStorage<AnalogRemotePoint> analogStorage)
        {
            commandingProcessors = new Dictionary<Type, ICommandingProcessor>()
            {
                { typeof(ChangeDiscreteRemotePointValue), new DiscreteRemotePointCommanding(discreteStorage) },
                { typeof(ChangeAnalogRemotePointValue), new AnalogRemotePointCommanding(analogStorage) }
            };
        }
    }
}
