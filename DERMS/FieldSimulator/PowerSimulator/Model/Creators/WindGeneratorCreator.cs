using FieldSimulator.PowerSimulator.Model.Equipment;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    class WindGeneratorCreator : BaseCreator<DERMS.WindGenerator, WindGenerator>
    {
        public WindGeneratorCreator(ImportHelper importHelper) : base(DMSType.WINDGENERATOR, importHelper)
        {
        }

        protected override WindGenerator InstantiateNewEntity(long globalId)
        {
            return new WindGenerator(globalId);
        }
    }
}
