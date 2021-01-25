using System.Runtime.Serialization;

namespace Common.SCADA.FieldProcessor
{
    [DataContract]
    public class ChangeRemotePointValueCommand : Command
    {
        public ChangeRemotePointValueCommand(long globalId, int commandingValue) : base(globalId)
        {
            CommandingValue = commandingValue;
        }

        [DataMember]
        public int CommandingValue { get; private set; }
    }
}
