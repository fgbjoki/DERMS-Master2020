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
using CalculationEngine.DERStates.CommandScheduler;
using CalculationEngine.Commanding.DERCommanding.TimedCommandCreator.EnergyStorage;
using CalculationEngine.Commanding.DERCommanding.TimedCommandCreator;
using CalculationEngine.DERStates.CommandScheduler.Commands;

namespace CalculationEngine.Commanding.DERCommanding
{
    public class DERCommandingProcessor : IDERCommandingProcessor
    {
        ISchedulerCommandExecutor scheduleCommandExecutor;

        private Dictionary<DMSType, DERCommandingWrapper> commandingUnits;
        private WCFClient<INDSCommanding> ndsCommandingClient;

        public DERCommandingProcessor(IDERStateDeterminator derStateDeterminator, IStorage<DistributedEnergyResource> DERStorage, ISchedulerCommandExecutor scheduleCommandExecutor)
        {
            this.scheduleCommandExecutor = scheduleCommandExecutor;

            InitializeCommandingPairs(derStateDeterminator, DERStorage);

            ndsCommandingClient = new WCFClient<INDSCommanding>("ndsCommanding");
        }

        private void InitializeCommandingPairs(IDERStateDeterminator derStateDeterminator, IStorage<DistributedEnergyResource> DERStorage)
        {
            commandingUnits = new Dictionary<DMSType, DERCommandingWrapper>()
            {
                {
                    DMSType.ENERGYSTORAGE,
                    new DERCommandingWrapper(new EnergyStorageCommandValidator(derStateDeterminator, DERStorage), new EnergyStorageCommandingUnit(DERStorage), new EnergyStorageSchedulerCommandCreator())
                }
            };
        }

        public CommandFeedback ValidateCommand(long derGid, float commandingValue)
        {
            DMSType derDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(derGid);

            DERCommandingWrapper commandingUnit;
            if (!commandingUnits.TryGetValue(derDMSType, out commandingUnit))
            {
                return new CommandFeedback()
                {
                    Successful = false,
                    Message = "Invalid DER gid."
                };
            }

            return commandingUnit.ValidateCommand(derGid, commandingValue);
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

            DERCommandingWrapper commandingUnit;
            if (!commandingUnits.TryGetValue(derDMSType, out commandingUnit))
            {
                return new CommandFeedback()
                {
                    Successful = false,
                    Message = "Invalid DER gid."
                };
            }

            Command ceCommand = commandingUnit.CreateCommand(derGid, commandingValue);
            if (ceCommand.CommandFeedback.Successful == false)
            {
                return ceCommand.CommandFeedback;
            }

            if (SendCommandToNDS(ceCommand.CreateNDSCommand()))
            {
                ScheduleCommand(ceCommand, commandingUnit);
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

        private void ScheduleCommand(Command ceCommand, ISchedulerCommandCreator timedCommandCreator)
        {
            SchedulerCommand schedulerCommand = timedCommandCreator.CreateSchedulerCommand(ceCommand);
            if (schedulerCommand == null)
            {
                return;
            }

            scheduleCommandExecutor.ExecuteCommand(schedulerCommand);
        }
    }
}
