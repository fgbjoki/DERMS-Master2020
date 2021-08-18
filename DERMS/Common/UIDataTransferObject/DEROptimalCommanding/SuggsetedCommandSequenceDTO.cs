using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.DEROptimalCommanding
{
    [DataContract]
    public class SuggestedCommand
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public float NominalPower { get; set; }

        [DataMember]
        public float ActivePower { get; set; }

        [DataMember]
        public float StateOfCharge { get; set; }

        [DataMember]
        public float DeltaActivePower { get; set; }

        [DataMember]
        public float CurrentLoad { get; set; }

        [DataMember]
        public float DeltaLoad { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public bool CommandValid { get; set; }

        [DataMember]
        public bool BatteryLow { get; set; }

        [DataMember]
        public bool HighLoad { get; set; }
    }

    [DataContract]
    public class SuggsetedCommandSequenceDTO
    {
        public SuggsetedCommandSequenceDTO()
        {
            SuggestedCommands = new List<SuggestedCommand>();
        }

        [DataMember]
        public List<SuggestedCommand> SuggestedCommands { get; set; }

        [DataMember]
        public bool CommandingSequenceValid { get; set; }
    }
}
