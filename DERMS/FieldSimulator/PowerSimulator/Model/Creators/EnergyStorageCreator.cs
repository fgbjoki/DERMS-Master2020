using FieldSimulator.PowerSimulator.Model.Equipment;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    class EnergyStorageCreator : BaseCreator<DERMS.EnergyStorage, EnergyStorage>
    {
        public EnergyStorageCreator(ImportHelper importHelper) : base(DMSType.ENERGYSTORAGE, importHelper)
        {
        }

        protected override EnergyStorage InstantiateNewEntity(long globalId)
        {
            return new EnergyStorage(globalId);
        }
    }
}
