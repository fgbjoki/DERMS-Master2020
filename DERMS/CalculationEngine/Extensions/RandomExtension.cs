using System;

namespace CalculationEngine.Extensions
{
    public static class RandomExtension
    {
        public static float Next(this Random random, float minimalValue, float maximualValue)
        {
            return Convert.ToSingle(random.NextDouble() * (maximualValue - minimalValue) + minimalValue);
        }
    }
}
