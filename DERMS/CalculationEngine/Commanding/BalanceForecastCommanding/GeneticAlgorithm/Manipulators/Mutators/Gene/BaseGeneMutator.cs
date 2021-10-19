using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Gene
{
    public abstract class BaseGeneMutator<T>
        where T : Model.Genes.Gene
    {
        protected static Random random = new Random();

        public abstract void InternalMutate(T gene);
    }
}
