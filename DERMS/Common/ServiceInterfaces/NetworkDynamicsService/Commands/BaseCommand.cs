using System.Runtime.Serialization;

namespace Common.ServiceInterfaces.NetworkDynamicsService.Commands
{
    [DataContract]
    [KnownType(typeof(ChangeAnalogRemotePointValue))]
    [KnownType(typeof(ChangeDiscreteRemotePointValue))]
    public abstract class BaseCommand
    {
        public BaseCommand()
        {

        }

        public BaseCommand(long globalId)
        {
            GlobalId = globalId;
        }

        [DataMember]
        public long GlobalId { get; set; }
    }
}
