using CalculationEngine.Commanding.Commands;

namespace CalculationEngine.Commanding.DERCommanding.Commanding
{
    public interface IDERCommandCreator
    {
        Command CreateCommand(long derGid, float commandingValue);
    }
}
