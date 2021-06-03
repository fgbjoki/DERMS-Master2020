using Common.DataTransferObjects.CalculationEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.Forecast.Production
{
    [DataContract]
    public class GeneratorProductionForecast
    {
        [DataMember]
        public long GlobalId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public float ActivePower { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }
    }

    [DataContract]
    public class TotalProductionForecast
    {
        public TotalProductionForecast()
        {
            TotalForecast = new List<Tuple<DateTime, float>>();
        }

        [DataMember]
        public List<Tuple<DateTime, float>> TotalForecast { get; set; }
    }

    [DataContract]
    public class ProductionForecastDTO
    {
        [DataMember]
        public TotalProductionForecast TimedForecast { get; set; }

        [DataMember]
        public Dictionary<long, List<GeneratorProductionForecast>> GeneratorProductionForecasts { get; set; }
    }
}
