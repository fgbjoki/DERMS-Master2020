using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Selectors;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators
{
    public class ManipulatorsWrapper<T>
        where T : Gene, new()
    {
        public BaseChromosomeSelector<T> ChromosomeSelector { get; set; }
        public BaseGeneMutator<T> Mutator { get; set; }
        public BaseChromosomeCreator<T> GeneCreator { get; set; }
    }
}
