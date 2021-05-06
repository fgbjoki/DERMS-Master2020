using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;
using UIAdapter.Model.DERGroup;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup
{
    public class WindGeneratorStorageItemCreator : GeneratorStorageItemCreator
    {
        public WindGeneratorStorageItemCreator() : base()
        {
        }

        protected override Generator InstantiateGenerator(ResourceDescription rd)
        {
            WindGenerator windGenerator = new WindGenerator(rd.Id);
            windGenerator.CutOutSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_CUTOUTSPEED).AsFloat();
            windGenerator.StartUpSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_STARTUPSPEED).AsFloat();
            windGenerator.NominalSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_NOMINALSPEED).AsFloat();

            return windGenerator;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.WINDGENERATOR,
                    new List<ModelCode>()
                    {
                        ModelCode.IDOBJ_NAME,
                        ModelCode.DER_NOMINALPOWER,
                        ModelCode.GENERATOR_ENERGYSTORAGE,
                        ModelCode.WINDGENERATOR_CUTOUTSPEED,
                        ModelCode.WINDGENERATOR_NOMINALSPEED,
                        ModelCode.WINDGENERATOR_STARTUPSPEED,
                        ModelCode.PSR_MEASUREMENTS
                    }
                },
            };
        }
    }
}
