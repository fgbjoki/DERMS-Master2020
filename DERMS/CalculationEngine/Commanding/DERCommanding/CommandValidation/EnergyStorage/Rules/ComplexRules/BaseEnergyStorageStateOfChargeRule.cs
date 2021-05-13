using CalculationEngine.Commanding.DERCommanding.CommandValidation.Rules;
using System;
using System.Text;

namespace CalculationEngine.Commanding.DERCommanding.CommandValidation.EnergyStorage.Rules.ComplexRules
{
    public abstract class BaseEnergyStorageStateOfChargeRule : BaseValidationRule<Model.DERCommanding.EnergyStorage>
    {
        protected string CreateStringChargingTime(float capacityToUse, float activePower)
        {
            return CreateStringChargingTime(CalculateSecondsOfStorageUsage(capacityToUse, activePower));
        }

        protected double CalculateSecondsOfStorageUsage(float capacity, float commandedActivePower)
        {
            double activePowerCapacity = capacity * 3600;

            double secondsOfUse = activePowerCapacity / Math.Abs(commandedActivePower);

            return secondsOfUse;
        }

        protected string CreateStringChargingTime(double seconds)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var chargingTime = TimeSpan.FromSeconds(seconds);

            if (chargingTime.Days > 0)
            {
                stringBuilder.Append($"{chargingTime.Days} day");
                if (chargingTime.Days > 1)
                {
                    stringBuilder.Append("s");
                }

                stringBuilder.Append(" ");
            }
            if (chargingTime.Hours > 0)
            {
                stringBuilder.Append($"{chargingTime.Hours} hour");
                if (chargingTime.Hours > 1)
                {
                    stringBuilder.Append("s");
                }

                stringBuilder.Append(" ");
            }
            if (chargingTime.Minutes > 0)
            {
                stringBuilder.Append($"{chargingTime.Minutes} minute");
                if (chargingTime.Minutes > 1)
                {
                    stringBuilder.Append("s");
                }

                stringBuilder.Append(" ");
            }
            if (chargingTime.Seconds > 0)
            {
                stringBuilder.Append($"{chargingTime.Seconds} second");
                if (chargingTime.Seconds > 1)
                {
                    stringBuilder.Append("s");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
