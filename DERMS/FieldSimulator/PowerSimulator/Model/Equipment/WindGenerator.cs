using FieldSimulator.PowerSimulator.Calculations;
using FieldSimulator.PowerSimulator.Model.Measurements;
using System.Linq;

namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    public class WindGenerator : Generator
    {
        public WindGenerator(long globalId) : base(globalId)
        {
        }

        public float CutOutSpeed { get; set; }

        public float StartUpSpeed { get; set; }

        public float NominalSpeed { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.WindGenerator cimWindGenerator = cimObject as DERMS.WindGenerator;

            if (cimWindGenerator == null)
            {
                return;
            }

            CutOutSpeed = cimWindGenerator.CutOutSpeed;
            NominalSpeed = cimWindGenerator.NominalSpeed;
            StartUpSpeed = cimWindGenerator.StartUpSpeed;
        }

        public override Calculation CreateCalculation()
        {
            WindGeneratorProductionCalculation calculation = new WindGeneratorProductionCalculation(StartUpSpeed, CutOutSpeed, NominalSpeed, NominalPower);
            PopulateActivePowerOutputParameter(calculation);

            return calculation;
        }

        private void PopulateActivePowerOutputParameter(Calculation calculation)
        {
            foreach (var analogMeasurement in Measurements.Cast<AnalogMeasurement>())
            {
                if (analogMeasurement.MeasurementType == MeasurementType.ActivePower)
                {
                    calculation.AddOutput(analogMeasurement.RemotePointType, analogMeasurement.Address);
                    return;
                }
            }
        }
    }
}
