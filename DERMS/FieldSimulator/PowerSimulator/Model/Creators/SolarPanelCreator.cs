using FieldSimulator.PowerSimulator.Model.Equipment;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class SolarPanelCreator : BaseCreator<DERMS.SolarGenerator, SolarPanel>
    {
        public SolarPanelCreator(ImportHelper importHelper) : base(DMSType.SOLARGENERATOR, importHelper)
        {
        }

        protected override SolarPanel InstantiateNewEntity(long globalId)
        {
            return new SolarPanel(globalId);
        }
    }
}
