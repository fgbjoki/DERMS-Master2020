using System.Runtime.Serialization;

namespace Core.Common.FEP.CommandingService
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
