namespace CalculationEngine.TopologyAnalysis
{
    public interface ITopologyModifier
    {
        void Write(long discreteRemotePointGid, int rawValue);
    }
}
