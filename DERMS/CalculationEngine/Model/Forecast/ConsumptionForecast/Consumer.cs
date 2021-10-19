using Common.ComponentStorage;

namespace CalculationEngine.Model.Forecast.ConsumptionForecast
{
    public class Consumer : IdentifiedObject
    {
        public Consumer(long globalId) : base(globalId)
        {

        }

        public Common.AbstractModel.ConsumerType Type { get; set; }

        public float Pfixed { get; set; }
    }
}
