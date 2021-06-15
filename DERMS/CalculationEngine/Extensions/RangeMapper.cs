namespace CalculationEngine.Extensions
{
    public static class RangeMapper
    {
        public static float MapRange(this float value, float sourceBegin, float sourceEnd, float destinationBegin, float destinationEnd)
        {
            return (value - sourceBegin) / (sourceEnd - sourceBegin) * (destinationEnd - destinationBegin) + destinationBegin;
        }
    }
}
