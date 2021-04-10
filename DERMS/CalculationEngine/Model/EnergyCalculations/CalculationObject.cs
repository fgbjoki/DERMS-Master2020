using Common.ComponentStorage;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Model.EnergyCalculations
{
    public enum CalculationType
    {
        ActivePower = 0
    }

    public struct CalculationWrapper
    {
        public long GlobalId { get; set; }

        public CalculationType CalculationType { get; set; }

        public float Value { get; set; }
    }

    public class CalculationObject : IdentifiedObject
    {
        private List<CalculationWrapper> calculations;

        public CalculationObject(long globalId) : base(globalId)
        {
            calculations = new List<CalculationWrapper>();
        }

        public CalculationWrapper GetCalculation(long globalId)
        {
            return calculations.FirstOrDefault(x => x.GlobalId == globalId);
        }

        public CalculationWrapper GetCalculation(CalculationType calculationType)
        {
            return calculations.FirstOrDefault(x => x.CalculationType == calculationType);
        }

        public CalculationWrapper GetCalculation(long globalId, CalculationType calculationType)
        {
            return calculations.FirstOrDefault(x => x.GlobalId == globalId && x.CalculationType == calculationType);
        }
        
        public bool CalculationExists(long calculationGid, CalculationType calculationType)
        {
            return calculations.Exists(x => x.GlobalId == calculationGid && x.CalculationType == calculationType);
        }

        public bool AddCalculation(long calculationGid, CalculationType calculationType, float value = 0)
        {
            if (CalculationExists(calculationGid, calculationType))
            {
                return false;
            }

            CalculationWrapper newCalculation = new CalculationWrapper()
            {
                GlobalId = calculationGid,
                CalculationType = calculationType,
                Value = value
            };

            calculations.Add(newCalculation);

            return true;
        }
    }
}
