using Common.AbstractModel;
using FieldSimulator.PowerSimulator.Model.Equipment;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class ACLineSegmentCreator : BaseCreator<DERMS.ACLineSegment, ACLineSegment>
    {
        public ACLineSegmentCreator(ImportHelper importHelper) : base(DMSType.ACLINESEG, importHelper)
        {
        }

        protected override ACLineSegment InstantiateNewEntity(long globalId)
        {
            return new ACLineSegment(globalId);
        }
    }
}
