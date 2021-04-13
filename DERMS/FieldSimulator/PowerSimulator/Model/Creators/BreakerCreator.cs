using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Equipment;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class BreakerCreator : BaseCreator<DERMS.Breaker, Breaker>
    {
        public BreakerCreator(ImportHelper importHelper) : base(DMSType.BREAKER, importHelper)
        {
        }

        protected override Breaker InstantiateNewEntity(long globalId)
        {
            return new Breaker(globalId);
        }
    }
}
