using Common.AbstractModel;

namespace UIAdapter.Model.DERGroup
{
    public enum GeneratorType
    {
        WIND,
        SOLAR
    }

    public class Generator : DistributedEnergyResource
    {
        public Generator(long globalId) : base(globalId)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            if (dmsType == DMSType.WINDGENERATOR)
            {
                GeneratorType = GeneratorType.WIND;
            }
            else if (dmsType == DMSType.SOLARGENERATOR)
            {
                GeneratorType = GeneratorType.SOLAR;
            }
        }

        public GeneratorType GeneratorType { get; set; }

        public long EnergyStorageGid { get; set; }
    }
}
