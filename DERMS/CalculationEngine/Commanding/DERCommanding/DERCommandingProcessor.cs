using CalculationEngine.Commanding.DERCommanding.Commanding.EnergyStorage;
using CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage;
using CalculationEngine.Model.DERCommanding;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;
using CalculationEngine.Commanding.Commands;
using Common.Communication;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using Common.DataTransferObjects.CalculationEngine;

namespace CalculationEngine.Commanding.DERCommanding
{
    public class DERCommandingProcessor : IDERCommandingProcessor
    {
        private Dictionary<DMSType, DERCommandingPair> commandingPairs;
        private WCFClient<INDSCommanding> ndsCommandingClient;

        public DERCommandingProcessor(IDERStateDeterminator derStateDeterminator, IStorage<DistributedEnergyResource> DERStorage)
        {
            InitializeCommandingPairs(derStateDeterminator, DERStorage);

            ndsCommandingClient = new WCFClient<INDSCommanding>("ndsCommanding");
        }

        private void InitializeCommandingPairs(IDERStateDeterminator derStateDeterminator, IStorage<DistributedEnergyResource> DERStorage)
        {
            commandingPairs = new Dictionary<DMSType, DERCommandingPair>()
            {
                {
                    DMSType.ENERGYSTORAGE,
                    new DERCommandingPair(new EnergyStorageCommandValidator(derStateDeterminator, DERStorage), new EnergyStorageCommandingUnit(DERStorage))
                }
            };
        }

        public CommandFeedback ValidateCommand(long derGid, float commandingValue)
        {
            DMSType derDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(derGid);

            DERCommandingPair commandingPair;
            if (!commandingPairs.TryGetValue(derDMSType, out commandingPair))
            {
                return new CommandFeedback()
                {
                    Successful = false,
                    Message = "Invalid DER gid."
                };
            }

            return commandingPair.ValidateCommand(derGid, commandingValue);
        }

        public CommandFeedback Command(long derGid, float commandingValue)
        {
            CommandFeedback validationFeedback = ValidateCommand(derGid, commandingValue);
            if (validationFeedback.Successful == false)
            {
                return validationFeedback;
            }

            return ProcessCommanding(derGid, commandingValue);
        }
        
        private CommandFeedback ProcessCommanding(long derGid, float commandingValue)
        {
            DMSType derDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(derGid);

            DERCommandingPair commandingPair;
            if (!commandingPairs.TryGetValue(derDMSType, out commandingPair))
            {
                return new CommandFeedback()
                {
                    Successful = false,
                    Message = "Invalid DER gid."
                };
            }

            Command ceCommand = commandingPair.CreateCommand(derGid, commandingValue);
            if (ceCommand.CommandFeedback.Successful == false)
            {
                return ceCommand.CommandFeedback;
            }

            if (SendCommandToNDS(ceCommand.CreateNDSCommand()))
            {
                return ceCommand.CommandFeedback;
            }
            else
            {
                return new CommandFeedback()
                {
                    Successful = false,
                    Message = "Couldn't send command. Check logs for more info."
                };
            }
        }

        private bool SendCommandToNDS(BaseCommand ndsCommand)
        {
            try
            {
                return ndsCommandingClient.Proxy.SendCommand(ndsCommand);
            }
            catch
            {
                return false;
            }
        }
    }
}
