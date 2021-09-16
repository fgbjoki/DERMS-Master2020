using Common.AbstractModel;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.Model.Genes
{
    public abstract class DERGene : Gene
    {
        private long globalId;

        public long GlobalId
        {
            get { return globalId; }
            set
            {
                globalId = value;
                DMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
            }
        }

        public DMSType DMSType { get; private set; }
        public float NominalPower { get; set; }
        public float ActivePower { get; set; }

        public override void PopulateGene(Gene g)
        {
            base.PopulateGene(g);
            DERGene derGene = g as DERGene;
            if (derGene == null)
            {
                return;
            }

            derGene.ActivePower = ActivePower;
            derGene.NominalPower = NominalPower;
            derGene.GlobalId = GlobalId;
        }
    }
}
