using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model;
using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Manipulators.Mutators.Chromosome
{
    public abstract class BaseChromosomeMutator<T>
        where T : Model.Genes.Gene
    {
        protected static Random random = new Random();
        protected float geneMutationRate = 0.2f;

        public abstract void Mutate(Chromosome<T> chromosome);
    }
}
