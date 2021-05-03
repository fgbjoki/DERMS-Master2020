using System.Runtime.Serialization;

namespace Common.ServiceInterfaces.NetworkDynamicsService.Commands
{
    [DataContract]
    public class ChangeAnalogRemotePointValue : BaseCommand
    {
        public ChangeAnalogRemotePointValue()
        {

        }

        public ChangeAnalogRemotePointValue(long globalId, float value) : base(globalId)
        {
            Value = value;
        }

        [DataMember]
        public float Value { get; set; }
    }
}
