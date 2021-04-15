using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Measurements;

namespace FieldSimulator.Modbus.SchemaAligner.MeasurementAligner
{
    class AnalogMeasurementAligner : BaseMeasurementAligner<AnalogPointWrapper>
    {
        protected override void PopulateValue(AnalogPointWrapper slaveModel, Measurement cimModel)
        {
            AnalogMeasurement analogCim = cimModel as AnalogMeasurement;
            slaveModel.FloatValue = analogCim.Value;
        }
    }
}
