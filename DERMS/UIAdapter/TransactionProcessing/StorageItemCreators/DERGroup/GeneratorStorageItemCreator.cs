using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.DERGroup;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup
{
    public abstract class GeneratorStorageItemCreator : StorageItemCreator
    {
        protected GeneratorStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            Generator generator = InstantiateGenerator(rd);
            PopulateProperties(generator, rd);

            return generator;
        }

        protected abstract Generator InstantiateGenerator(ResourceDescription rd);

        protected void PopulateProperties(Generator generator, ResourceDescription rd)
        {
            generator.NominalPower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();
            generator.EnergyStorageGid = rd.GetProperty(ModelCode.GENERATOR_ENERGYSTORAGE).AsReference();
            generator.Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString();
        }
    }
}
