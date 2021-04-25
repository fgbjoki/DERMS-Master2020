using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.DERGroup;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup
{
    public class GeneratorStorageItemCreator : StorageItemCreator
    {
        public GeneratorStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            Generator generator = new Generator(rd.Id);

            PopulateEnergyStorageProperties(generator, rd);
            generator.Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString();

            return generator;
        }

        private void PopulateEnergyStorageProperties(Generator generator, ResourceDescription rd)
        {
            generator.EnergyStorageGid = rd.GetProperty(ModelCode.GENERATOR_ENERGYSTORAGE).AsReference();
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.WINDGENERATOR,
                    new List<ModelCode>() { ModelCode.IDOBJ_NAME, ModelCode.GENERATOR_ENERGYSTORAGE }
                },
                {
                    DMSType.SOLARGENERATOR,
                    new List<ModelCode>() { ModelCode.IDOBJ_NAME, ModelCode.GENERATOR_ENERGYSTORAGE }
                },
            };
        }
    }
}
