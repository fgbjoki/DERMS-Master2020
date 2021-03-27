using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using System.Linq;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model.Schema;
using Common.Logger;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.Schema
{
    class EnergySourceItemCreator : StorageItemCreator
    {
        public EnergySourceItemCreator() : base(NeededProperties())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            EnergySource energySource = new EnergySource(rd.Id);

            if (!PopulateSubstationProperties(energySource, rd, affectedEntities))
            {
                return null;
            }

            return energySource;
        }

        private bool PopulateSubstationProperties(EnergySource energySource, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            PopulateSubstationGid(energySource, rd);
            return PopulateSubstationName(energySource, rd, affectedEntities);
        }

        private bool PopulateSubstationName(EnergySource energySource, ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            List<ResourceDescription> substations;

            if (!affectedEntities.TryGetValue(DMSType.SUBSTATION, out substations))
            {
                Logger.Instance.Log($"[{GetType()}] Cannot find substation with gid {energySource.SubstationGid:X16}.");
                return false;
            }

            ResourceDescription substationRd = substations.FirstOrDefault(x => x.Id == energySource.SubstationGid);

            if (substationRd == null)
            {
                Logger.Instance.Log($"[{GetType()}] Cannot find substation with gid {energySource.SubstationGid:X16}.");
                return false;
            }

            Property substationNameProperty = substationRd.GetProperty(ModelCode.IDOBJ_NAME);

            if (substationNameProperty == null)
            {
                Logger.Instance.Log($"[{GetType()}] Substation with gid {energySource.SubstationGid:X16} does not have a name property!");
                return false;
            }

            energySource.SubstationName = substationNameProperty.AsString();
            return true;
        }

        private void PopulateSubstationGid(EnergySource energySource, ResourceDescription rd)
        {
            Property substationGidProperty = rd.GetProperty(ModelCode.EQUIPMENT_EQCONTAINER);

            long substationGid = 0;

            if (substationGidProperty != null)
            {
                substationGid = substationGidProperty.AsReference();
            }

            energySource.SubstationGid = substationGid;
        }

        private static Dictionary<DMSType, List<ModelCode>> NeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.ENERGYSOURCE, new List<ModelCode>(1)
                    {
                        ModelCode.EQUIPMENT_EQCONTAINER
                    }
                },
                { DMSType.SUBSTATION, new List<ModelCode>(1)
                    {
                        ModelCode.IDOBJ_NAME
                    }
                },
            };
        }
    }
}
