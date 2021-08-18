using System.Runtime.Serialization;

namespace Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding
{
    [DataContract]
    [KnownType(typeof(NominalPowerDEROptimalCommand))]
    [KnownType(typeof(UniformReserveDEROptimalCommand))]
    public abstract class DEROptimalCommand
    {
        [DataMember]
        public float SetPoint { get; set; }
    }
}
