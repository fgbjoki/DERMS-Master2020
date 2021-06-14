using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using System;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.InitialPopulationCreators.GeneCreators
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
                T newGene = CreateNewGene(entity);
                genes.Add(newGene);
            }

            return genes;
        }

        protected abstract T CreateNewGene(TT entity);
    }
}
