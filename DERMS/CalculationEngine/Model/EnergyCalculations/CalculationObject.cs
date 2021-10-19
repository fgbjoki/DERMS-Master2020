using Common.ComponentStorage;
using System.Collections.Generic;

namespace CalculationEngine.Model.EnergyCalculations
{
    public enum CalculationType
    {
        ActivePower = 0
    }

    public class CalculationWrapper
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
            foreach (var calculation in calculations)
            {
                if (calculation.GlobalId == globalId)
                {
                    return calculation;
                }
            }

            return null;
        }

        public CalculationWrapper GetCalculation(CalculationType calculationType)
        {
            foreach (var calculation in calculations)
            {
                if (calculation.CalculationType == calculationType)
                {
                    return calculation;
                }
            }

            return null;
        }

        public CalculationWrapper GetCalculation(long globalId, CalculationType calculationType)
        {
            foreach (var calculation in calculations)
            {
                if (calculation.CalculationType == calculationType && calculation.GlobalId == globalId)
                {
                    return calculation;
                }
            }

            return null;
        }
        
        public bool CalculationExists(long calculationGid, CalculationType calculationType)
        {
            foreach (var calculation in calculations)
            {
                if (calculation.CalculationType == calculationType && calculation.GlobalId == calculationGid)
                {
                    return true;
                }
            }

            return false;
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
