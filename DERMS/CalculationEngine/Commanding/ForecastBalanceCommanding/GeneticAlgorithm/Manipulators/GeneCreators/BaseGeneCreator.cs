using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using System;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.GeneCreators
{
    public abstract class BaseGeneCreator<T>
        where T : Gene
    {
        protected Random random = new Random();
        public abstract void Crossover(T child, T firstParent, T secondParent);
    }
}
