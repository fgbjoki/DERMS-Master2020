using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model
{
    public class Population<T>
        where T : Gene
    {
        public Population()
        {
            Chromosomes = new List<Chromosome<T>>();
        }

        public List<Chromosome<T>> Chromosomes { get; set; }
    }
}
