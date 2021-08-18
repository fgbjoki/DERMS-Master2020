using CalculationEngine.Commanding.DEROptimalCommanding.CommandingProcessors;
using CalculationEngine.Model.DERCommanding;
using CalculationEngine.Model.DERStates;
using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine.DEROptimalCommanding;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.DEROptimalCommanding
{
    public class DEROptimalCommandingProcessor : IDEROptimalCommanding
    {
        private IDERCommandingProcessor derCommandingProcessor;
        private Dictionary<Type, IDEROptimalCommandingProcessor> derOptimalCommandingProcessors;

        public DEROptimalCommandingProcessor(IDERCommandingProcessor derCommandingProcessor, IStorage<DERState> derStateStorage, IStorage<DistributedEnergyResource> derStorage)
        {
            this.derCommandingProcessor = derCommandingProcessor;
            InitializeOptimalCommandingProcessors(derStateStorage, derStorage);
        }

        public DEROptimalCommandingFeedbackDTO CreateCommand(DEROptimalCommand command)
        {
            IDEROptimalCommandingProcessor optimalCommandingProcessor;
            if (!derOptimalCommandingProcessors.TryGetValue(command.GetType(), out optimalCommandingProcessor))
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find optimal processor for command of type: \'{command.GetType().Name}\'");
                return null;
            }

            DEROptimalCommandingFeedbackDTO commandingFeedback = new DEROptimalCommandingFeedbackDTO();

            var suggestedValues = optimalCommandingProcessor.CreateCommandSequence(command);
            foreach (var suggestedValue in suggestedValues)
            {
                var feedback = derCommandingProcessor.ValidateCommand(suggestedValue.GlobalId, suggestedValue.ActivePower);
                commandingFeedback.Result.Add(new DERUnitFeedbackDTO()
                {
                    DERGlobalId = suggestedValue.GlobalId,
                    ActivePower = suggestedValue.ActivePower,
                    CommandFeedback = feedback
                });

                commandingFeedback.ValidCommanding &= feedback.Successful;
            }

            return commandingFeedback;
        }

        private void InitializeOptimalCommandingProcessors(IStorage<DERState> derStateStorage, IStorage<DistributedEnergyResource> derStorage)
        {
            derOptimalCommandingProcessors = new Dictionary<Type, IDEROptimalCommandingProcessor>()
            {
                { typeof(NominalPowerDEROptimalCommand), new NominalPowerPercentageOptimalCommandingProcessor(derStateStorage, derStorage) },
                { typeof(UniformReserveDEROptimalCommand), new UniformReserveUsageDEROptimalCommandingProcessor(derStateStorage, derStorage) }
            };
        }
    }
}
