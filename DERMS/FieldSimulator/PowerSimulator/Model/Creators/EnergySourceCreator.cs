using System;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class EnergySourceCreator : BaseCreator<DERMS.EnergySource, Equipment.EnergySource>
    {
        public EnergySourceCreator(ImportHelper importHelper) : base(DMSType.ENERGYSOURCE, importHelper)
        {
        }

        protected override Equipment.EnergySource InstantiateNewEntity(long globalId)
        {
            return new Equipment.EnergySource(globalId);
        }
    }
}
