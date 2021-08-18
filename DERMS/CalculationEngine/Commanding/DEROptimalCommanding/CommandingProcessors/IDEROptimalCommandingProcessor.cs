using System.Collections.Generic;
using Common.ServiceInterfaces.CalculationEngine.DEROptimalCommanding;

namespace CalculationEngine.Commanding.DEROptimalCommanding.CommandingProcessors
{
    public interface IDEROptimalCommandingProcessor
    {
        List<SuggestedDERValues> CreateCommandSequence(DEROptimalCommand command);
    }
}