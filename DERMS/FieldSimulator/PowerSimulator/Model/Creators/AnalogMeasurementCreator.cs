using FieldSimulator.PowerSimulator.Model.Measurements;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class AnalogMeasurementCreator : BaseCreator<DERMS.Analog, AnalogMeasurement>
    {
        public AnalogMeasurementCreator(ImportHelper importHelper) : base(DMSType.MEASUREMENTANALOG, importHelper)
        {
        }

        protected override AnalogMeasurement InstantiateNewEntity(long globalId)
        {
            return new AnalogMeasurement(globalId);
        }
    }
}
