using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Parameters;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm
{
    public class EnergyBalanceGeneticAlgorithm : GeneticAlgorithm<GridStateGene, EnergyBalanceGeneticAlgorithmParameters>
    {
        public EnergyBalanceGeneticAlgorithm(EnergyBalanceGeneticAlgorithmParameters parameters) : base(parameters)
        {
        }

        protected override BaseChromosomeCreator<GridStateGene> GetChromosomeCreator()
        {
            return parameters.ManipulatorWrapper.GeneCreator;
        }

        protected override BaseGeneMutator<GridStateGene> GetGeneMutator()
        {
            return parameters.ManipulatorWrapper.Mutator;
        }

        protected override BaseChromosomeSelector<GridStateGene> GetSelector()
        {
            return parameters.ManipulatorWrapper.ChromosomeSelector;
        }

        protected override void PopulateFitnessParameters()
        {
            base.PopulateFitnessParameters();
            
        }
    }
}
