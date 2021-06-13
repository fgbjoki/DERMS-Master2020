using System.Collections.Generic;
using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes
{
    public class GridStateGene : EnergyBalanceGene
    {
        public GridStateGene()
        {
            DERGenes = new List<DERGene>();
        }

        public List<DERGene> DERGenes { get; set; }

        public override void PopulateValues(Gene g)
        {
            base.PopulateValues(g);
            GridStateGene gridStateGene = g as GridStateGene;
            if (gridStateGene == null)
            {
                return;
            }

            for (int i = 0; i < DERGenes.Count; i++)
            {
                DERGenes[i].PopulateValues(gridStateGene.DERGenes[i]);
            }
        }
    }
}
