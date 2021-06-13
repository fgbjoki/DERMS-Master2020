using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Helpers.FitnessParameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.FitnessCalculators.EnergyBalance;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators.EnergyBalance;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors.EnergyBalanceSelectors;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters
{
    public class EnergyBalanceGeneticAlgorithmParameters : BaseGeneticAlgorithmParameters<GridStateGene>
    {
        public EnergyBalanceGeneticAlgorithmParameters(float lowerBoundStateOfCharge = 0.2f, float upperBoundStateOfCharge = 1f, ulong simulationInterval = 15 * 60)
        {
            FitnessParameters = new EnergyBalanceFitnessparamter();

            EnergyStorageActivePowerCalculation esAPCalculator = new EnergyStorageActivePowerCalculation(lowerBoundStateOfCharge, upperBoundStateOfCharge, simulationInterval);
            GridStateFitnessCalculator gridStateFitnessCalculator = new GridStateFitnessCalculator(FitnessParameters);

            ManipulatorWrapper = new ManipulatorsWrapper<GridStateGene>()
            {
                ChromosomeSelector = new EnergyBalanceSelector(gridStateFitnessCalculator),
                Mutator = new GridStateGeneMutator(esAPCalculator),
                GeneCreator = new GridStateChromosomeCreator(new GridStateGeneCreator(esAPCalculator))
            };
        }


        public EnergyBalanceFitnessparamter FitnessParameters { get; set; }
    }
}
