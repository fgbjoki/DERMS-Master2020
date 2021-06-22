using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production
{
    public class GeneratorProduction : DEREntity
    {
        public GeneratorProduction(long globalId, float activePower)
        {
            GlobalId = globalId;
            ActivePower = activePower;
            NominalPower = 0;
        }
    }
}
