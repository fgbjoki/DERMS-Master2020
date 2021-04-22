namespace CalculationEngine.TopologyAnalysis.InterConnectedBreakerCommanding
{
    interface IInterConnectedBreakerCommanding
    {
        void ProcessBreakerCommanding(long breakerGidCommanding, int rawBreakerValue);
        void UpdateBreakers();
    }
}