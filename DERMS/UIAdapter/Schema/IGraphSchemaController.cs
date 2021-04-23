namespace UIAdapter.Schema
{
    public interface IGraphSchemaController
    {
        void ProcessDiscreteValueChanges(long discreteGid, int value);
    }
}