using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine
{
    [DataContract]
    public class ForecastDTO
    {
        [DataMember]
        public Dictionary<DateTime, List<ForecastSampleDTO>> ProductionForecastSamples { get; set; }
    }
}
