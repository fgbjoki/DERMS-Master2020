using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.GeneCreators
{
    public abstract class BaseInitialGeneCreator<T, TT>
        where T : Gene
    {
        protected static Random random = new Random();

        public virtual List<T> CreateGene(TT entity, int numberOfGenesToCreate)
        {
            List<T> genes = new List<T>(numberOfGenesToCreate);

            for (int i = 0; i < numberOfGenesToCreate; i++)
            {
                T newGene = CreateNewGene();
                PopulateFields(newGene, entity);
                genes.Add(newGene);
            }

            return genes;
        }

        protected abstract T CreateNewGene();

        protected abstract void PopulateFields(T gene, TT entity);

    }
}
