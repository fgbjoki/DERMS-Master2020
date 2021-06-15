using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using System;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Manipulators.Mutators
{
    public abstract class BaseGeneMutator<T>
        where T : Gene
    {
        protected static Random random = new Random();

        public abstract void Mutate(T gene);
    }
}
