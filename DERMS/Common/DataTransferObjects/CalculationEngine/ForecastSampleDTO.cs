using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine
{
    [DataContract]
    public class ForecastSampleDTO
    {
        [DataMember]
        public long GeneratorGid { get; set; }

        [DataMember]
        public float ActivePower { get; set; }
    }
}
