namespace CalculationEngine.Model.Forecast.ProductionForecast
{
    public class WindGenerator : Generator
    {
        public WindGenerator(long globalId) : base(globalId)
        {
        }

        public float StartUpSpeed { get; set; }
        public float CutOutSpeed { get; set; }
        public float NominalSpeed { get; set; }
    }
}
