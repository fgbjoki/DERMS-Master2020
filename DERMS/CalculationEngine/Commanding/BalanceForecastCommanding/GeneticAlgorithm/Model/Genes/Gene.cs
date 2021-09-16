using System;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes
{
    public abstract class Gene : ICloneable
    {
        protected abstract Gene ReplicateGene();

        public object Clone()
        {
            Gene newGene = ReplicateGene();
            PopulateGene(newGene);

            return newGene;
        }

        public virtual void PopulateGene(Gene g)
        {

        }
    }
}
