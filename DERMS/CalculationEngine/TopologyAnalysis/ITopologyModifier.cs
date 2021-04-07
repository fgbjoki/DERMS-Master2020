namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyWriter
    {
        void Write(long breakerGid, int rawValue);
    }
}
