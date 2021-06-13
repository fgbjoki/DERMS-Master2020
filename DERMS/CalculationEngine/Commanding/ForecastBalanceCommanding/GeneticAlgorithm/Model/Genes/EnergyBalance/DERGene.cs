using CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.Genes;
using Common.AbstractModel;

namespace CalculationEngine.Commanding.ForecastBalanceCommanding.GeneticAlgorithm.Model.EnergyBalance.Genes
{
    public abstract class DERGene : EnergyBalanceGene
    {
        private long globalId;

        protected DERGene(long globalId, float nominalPower, float activePower)
        {
            GlobalId = globalId;
            NominalPower = nominalPower;
            ActivePower = activePower;
        }

        public long GlobalId
        {
            get { return globalId; }
            set
            {
                globalId = value;
                DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(value);
            }
        }

        public DMSType DMSType { get; private set; }
        public float NominalPower { get; set; }
        public float ActivePower { get; set; }

        public override void PopulateValues(Gene g)
        {
            base.PopulateValues(g);

            DERGene derGene = g as DERGene;
            if (derGene == null)
            {
                return;
            }

            derGene.GlobalId = GlobalId;
            derGene.NominalPower = NominalPower;
            derGene.ActivePower = ActivePower;
        }
    }
}
