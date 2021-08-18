using Common.Communication;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.DataTransferObjects.CalculationEngine.DEROptimalCommanding;
using Common.ServiceInterfaces.CalculationEngine;
using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;
using Common.ServiceInterfaces.UIAdapter;
using Common.UIDataTransferObject.DEROptimalCommanding;
using System;
using UIAdapter.Model.DERGroup;
using UIAdapter.Model.NetworkModel;

namespace UIAdapter.Commanding.DEROptimalCommanding
{
    public class DEROptimalCommandingProxy : IDEROptimalCommandingProxy
    {
        private WCFClient<IDEROptimalCommanding> derOptimalCommanding;
        private IStorage<DERGroup> derGroupStorage;
        private IStorage<NetworkModelItem> networkModelStorage;
        private IDERCommanding derCommanding;

        public DEROptimalCommandingProxy(IDERCommanding derCommanding, IStorage<NetworkModelItem> networkModelStorage, IStorage<DERGroup> derGroupStorage)
        {
            this.derCommanding = derCommanding;
            this.networkModelStorage = networkModelStorage;
            this.derGroupStorage = derGroupStorage;
            derOptimalCommanding = new WCFClient<IDEROptimalCommanding>("ceDEROptimalCommanding");
        }

        public SuggsetedCommandSequenceDTO GetSuggestedCommandSequence(CommandRequestDTO commandSequenceRequest, float setPoint)
        {
            DEROptimalCommand ceCommand = CreateCECommand(commandSequenceRequest, setPoint);
            if (ceCommand == null)
            {
                return null;
            }

            var ceFeedback = GetSuggestedValue(ceCommand);
            if (ceFeedback == null)
            {
                return null;
            }

            SuggsetedCommandSequenceDTO clientResponse = new SuggsetedCommandSequenceDTO();
            clientResponse.CommandingSequenceValid = true;

            foreach (var ceSuggestion in ceFeedback.Result)
            {
                var suggestionCommand = CreateResponse(ceSuggestion);
                clientResponse.CommandingSequenceValid &= suggestionCommand.CommandValid;
                clientResponse.SuggestedCommands.Add(suggestionCommand);
            }

            return clientResponse;
        }

        public CommandFeedbackMessageDTO ExecuteCommandSequence(CommandSequenceRequest commandSequence)
        {
            int i = 0;
            bool successfulySentCommands = true;
            var feedback = new CommandFeedbackMessageDTO();
            for (; i < commandSequence.CommandRequestSequence.Count; ++i)
            {
                var command = commandSequence.CommandRequestSequence[i];
                try
                {
                    var commandFeedback = derCommanding.SendCommand(command.GlobalId, command.ActivePower);
                    if (commandFeedback.CommandExecuted == false)
                    {
                        feedback.CommandExecuted = false;
                        feedback.Message = $"Coudln't execute command on entity with gid: {command.GlobalId:X16}, Reason:\n" + commandFeedback.Message;
                    }
                }
                catch (Exception e)
                {
                    successfulySentCommands = false;
                    feedback.Message = $"Commands were reverted more info: {e.Message}";
                }
            }

            if (!successfulySentCommands)
            {
                for (int j = 0; j <= i; j++)
                {
                    try
                    {
                        var command = commandSequence.CommandRequestSequence[j];
                        var commandFeedback = derCommanding.SendCommand(command.GlobalId, 0);
                    }
                    catch { }
                }
            }

            if (successfulySentCommands)
            {
                feedback.CommandExecuted = true;
                feedback.Message = "Successfuly executed command sequence.";
            }

            return feedback;
        }

        private SuggestedCommand CreateResponse(DERUnitFeedbackDTO ceResponse)
        {
            SuggestedCommand suggestedCommand = new SuggestedCommand()
            {
                GlobalId = ceResponse.DERGlobalId,
                CommandValid = ceResponse.CommandFeedback.Successful,
                Comment = ceResponse.CommandFeedback.Message,
            };

            PopulateEntityWithStorageData(suggestedCommand);

            suggestedCommand.DeltaActivePower = ceResponse.ActivePower - suggestedCommand.ActivePower;
            suggestedCommand.DeltaLoad = (ceResponse.ActivePower / suggestedCommand.NominalPower * 100) - suggestedCommand.CurrentLoad;

            return suggestedCommand;
        }

        private void PopulateEntityWithStorageData(SuggestedCommand suggestedCommand)
        {
            EnergyStorage energyStorage = derGroupStorage.GetEntity(suggestedCommand.GlobalId).EnergyStorage;
            suggestedCommand.ActivePower = energyStorage.ActivePower;
            suggestedCommand.CurrentLoad = energyStorage.ActivePower / energyStorage.NominalPower * 100;
            suggestedCommand.StateOfCharge = energyStorage.StateOfCharge;
            suggestedCommand.BatteryLow = energyStorage.StateOfCharge <= 0.25;
            suggestedCommand.HighLoad = suggestedCommand.CurrentLoad >= 90;
            suggestedCommand.NominalPower = energyStorage.NominalPower;
            suggestedCommand.Name = energyStorage.Name;
        }

        private DEROptimalCommandingFeedbackDTO GetSuggestedValue(DEROptimalCommand commandRequest)
        {
            try
            {
                return derOptimalCommanding.Proxy.CreateCommand(commandRequest);
            }
            catch
            {
                return null;
            }
        }

        private DEROptimalCommand CreateCECommand(CommandRequestDTO clientRequest, float setPoint)
        {
            switch (clientRequest)
            {
                case CommandRequestDTO.NominalPower:
                    return new NominalPowerDEROptimalCommand() { SetPoint = setPoint };
                case CommandRequestDTO.Reserve:
                    return new UniformReserveDEROptimalCommand() { SetPoint = setPoint };
                default:
                    return null;
            }
        }
    }
}
