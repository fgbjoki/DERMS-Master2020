using System.Collections.Generic;
using System.Linq;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.DERGroup;
using Common.Logger;
using Common.ComponentStorage.StorageItemCreator;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERGroup
{
    public class DERGroupStorageItemCreator : StorageItemCreator
    {
        private EnergyStorageStorageItemCreator energyCreator = new EnergyStorageStorageItemCreator();
        private SolarPanelStorageItemCreator solarPanelCreator = new SolarPanelStorageItemCreator();
        private WindGeneratorStorageItemCreator windGeneratorCreator = new WindGeneratorStorageItemCreator();

        public DERGroupStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            Model.DERGroup.DERGroup derGroup = new Model.DERGroup.DERGroup(rd.Id);

            EnergyStorage energyStorage = energyCreator.CreateStorageItem(rd, affectedEntities) as EnergyStorage;

            long generatorGid = rd.GetProperty(ModelCode.ENERGYSTORAGE_GENERATOR).AsReference();
            ResourceDescription generatorRd = FindGeneratorRd(generatorGid, affectedEntities);
            if (generatorRd == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't find generator with gid: 0x{generatorGid:X16}.");
                return null;
            }

            GeneratorStorageItemCreator generatorCreator = GetGeneratorStorageItemCreator(generatorRd.Id);
            if (generatorCreator == null)
            {
                return null;
            }

            Generator generator = generatorCreator.CreateStorageItem(generatorRd, affectedEntities) as Generator;

            derGroup.EnergyStorage = energyStorage;
            derGroup.Generator = generator;

            return derGroup;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            Dictionary<DMSType, List<ModelCode>> properties = energyCreator.GetNeededProperties();

            foreach (var propertyPair in solarPanelCreator.GetNeededProperties().Concat(windGeneratorCreator.GetNeededProperties()))
            {
                List<ModelCode> modelCodes;
                if (!properties.TryGetValue(propertyPair.Key, out modelCodes))
                {
                    properties[propertyPair.Key] = propertyPair.Value;
                    continue;
                }

                foreach (var modelCode in propertyPair.Value)
                {
                    if (!modelCodes.Contains(modelCode))
                    {
                        modelCodes.Add(modelCode);
                    }
                }
            }

            return properties;
        }

        private ResourceDescription FindGeneratorRd(long generatorGid, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(generatorGid);

            return affectedEntities[dmsType].FirstOrDefault(x => x.Id == generatorGid);
        }

        private GeneratorStorageItemCreator GetGeneratorStorageItemCreator(long generatorGid)
        {
            DMSType generatorDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(generatorGid);
            if (generatorDMSType == DMSType.SOLARGENERATOR)
            {
                return solarPanelCreator;
            }
            else if (generatorDMSType == DMSType.WINDGENERATOR)
            {
                return windGeneratorCreator;
            }

            return null;
        }
    }
}
