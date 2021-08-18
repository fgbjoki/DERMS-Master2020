using ClientUI.CustomControls.DEROptimalFeedbackIcons;
using Common.UIDataTransferObject.DEROptimalCommanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.DEROptimalCommanding.OptimalCommanding.CommandSummaryViewModels
{
    public abstract class BaseSummarySuggestedValueViewModel : BaseViewModel
    {
        private long globalId;

        protected BaseSummarySuggestedValueViewModel()
        {
            Indications = new FeedbackIconCollectionViewModel();
        }

        public string Name { get; set; }

        public float NominalPower { get; set; }

        public float ActivePower { get; set; }

        public float StateOfCharge { get; set; }

        /// <summary>
        /// Delta power to be added to current active power.
        /// </summary>
        public float DeltaActivePower { get; set; }

        public float CurrentLoad { get; set; }

        public float DeltaLoad { get; set; }

        public string Comment { get; set; }

        public FeedbackIconCollectionViewModel Indications { get; }

        public virtual void PopulateFields(SuggestedCommand suggestedCommand)
        {
            globalId = suggestedCommand.GlobalId;

            Name = suggestedCommand.Name;
            Comment = suggestedCommand.Comment;
            DeltaLoad = suggestedCommand.DeltaLoad;
            ActivePower = suggestedCommand.ActivePower;
            CurrentLoad = suggestedCommand.CurrentLoad;
            NominalPower = suggestedCommand.NominalPower;
            StateOfCharge = suggestedCommand.StateOfCharge * 100;
            DeltaActivePower = suggestedCommand.DeltaActivePower;

            Indications.BatteryLow = suggestedCommand.BatteryLow;
            Indications.HighLoad = suggestedCommand.HighLoad;

            if (suggestedCommand.CommandValid)
            {
                Indications.Valid = true;
            }
            else
            {
                Indications.Error = true;
            }
        }

        public CommandRequest CreateCommandRequest()
        {
            return new CommandRequest()
            {
                GlobalId = globalId,
                ActivePower = ActivePower + DeltaActivePower
            };
        }
    }
}
