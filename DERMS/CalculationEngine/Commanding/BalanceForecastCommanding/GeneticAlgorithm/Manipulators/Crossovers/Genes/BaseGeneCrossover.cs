using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Crossovers.Genes
{
    public abstract class BaseGeneCrossover<T>
        where T : Gene
    {
        protected static Random random = new Random();

        public abstract T InternalCrossover(T firstParent, T secondParent);
    }
}
