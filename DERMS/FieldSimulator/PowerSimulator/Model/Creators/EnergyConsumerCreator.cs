using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    class EnergyConsumerCreator : BaseCreator<DERMS.EnergyConsumer, Equipment.EnergyConsumer>
    {
        public EnergyConsumerCreator(ImportHelper importHelper) : base(DMSType.ENERGYCONSUMER, importHelper)
        {
        }

        protected override Equipment.EnergyConsumer InstantiateNewEntity(long globalId)
        {
            return new Equipment.EnergyConsumer(globalId);
        }
    }
}
