using System.Runtime.Serialization;

namespace Core.Common.FEP.CommandingService
{
    [DataContract]
    [KnownType(typeof(ChangeRemotePointValueCommand))]
    public class Command
    {
        public Command(long globalId)
        {
            GlobalId = globalId;
        }

        [DataMember]
        public long GlobalId { get; private set; }
    }
}
