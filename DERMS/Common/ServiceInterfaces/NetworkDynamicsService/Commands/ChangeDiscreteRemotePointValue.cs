using System.Runtime.Serialization;

namespace Common.ServiceInterfaces.NetworkDynamicsService.Commands
{
    [DataContract]
    public class ChangeDiscreteRemotePointValue : BaseCommand
    {
        public ChangeDiscreteRemotePointValue()
        {

        }

        public ChangeDiscreteRemotePointValue(long globalId) : base(globalId)
        {
        }

        [DataMember]
        public int Value { get; set; }
    }
}
