using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators.EnergyBalance;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.ChromosomeCreators
{
    public class GridStateChromosomeCreator : BaseChromosomeCreator<GridStateGene>
    {
        private GridStateGeneCreator gridStateGeneCreator;

        public GridStateChromosomeCreator(GridStateGeneCreator gridStateGeneCreator)
        {
            this.gridStateGeneCreator = gridStateGeneCreator;
        }

        public override void Crossover(Chromosome<GridStateGene> child, Chromosome<GridStateGene> firstParent, Chromosome<GridStateGene> secondParent)
        {
            for (int i = 0; i < child.Genes.Count; i++)
            {
                GridStateGene childGene = child.Genes[i];
                GridStateGene firstParentGene = firstParent.Genes[i];
                GridStateGene secondParentGene = secondParent.Genes[i];

                gridStateGeneCreator.Crossover(childGene, firstParentGene, secondParentGene);
            }
        }
    }
}
