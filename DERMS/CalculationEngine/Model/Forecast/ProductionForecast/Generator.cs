using Common.ComponentStorage;

namespace CalculationEngine.Model.Forecast.ProductionForecast
{
    public abstract class Generator : IdentifiedObject
    {
        public Generator(long globalId) : base(globalId)
        {
        }

        public float NominalPower { get; set; }
    }
}
