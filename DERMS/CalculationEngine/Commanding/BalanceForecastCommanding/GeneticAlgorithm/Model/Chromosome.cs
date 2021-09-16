using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes;
using System.Collections.Generic;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model
{
    public class Chromosome<T>
        where T : Gene
    {
        public Chromosome()
        {
            Genes = new List<T>();
        }

        public List<T> Genes { get; set; }
        public float FitnessValue { get; set; }
        public float ImportedEnergy { get; set; }
    }
}
