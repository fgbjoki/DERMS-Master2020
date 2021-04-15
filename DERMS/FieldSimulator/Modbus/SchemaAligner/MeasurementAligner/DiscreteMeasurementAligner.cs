using FieldSimulator.Model;
using FieldSimulator.PowerSimulator.Model.Measurements;

namespace FieldSimulator.Modbus.SchemaAligner.MeasurementAligner
{
    class DiscreteMeasurementAligner : BaseMeasurementAligner<DiscretePointWrapper>
    {
        protected override void PopulateValue(DiscretePointWrapper slaveModel, Measurement cimModel)
        {
            DiscreteMeasurement discreteMeasurement = cimModel as DiscreteMeasurement;

            slaveModel.Value = discreteMeasurement.Value;
        }
    }
}
