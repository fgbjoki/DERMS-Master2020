using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Chromosomes
{
    public class Chromosome<T> 
        where T : Gene, new()
    {
        public Chromosome()
        {
            Genes = new List<T>();
        }

        public List<T> Genes { get; set; }

        public double FitnessValue { get; set; }

        public Chromosome<T> CreateNewChromosome()
        {
            Chromosome<T> newChromosome = new Chromosome<T>();

            foreach (var gene in Genes)
            {
                T newGene = new T();
                gene.PopulateValues(newGene);

                newChromosome.Genes.Add(newGene);
            }

            return newChromosome;
        }
    }
}
