namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyModifier
    {
        void Write(long breakerGid, int rawValue);
    }
}
