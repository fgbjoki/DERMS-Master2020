using Common.AbstractModel;

namespace CalculationEngine.Model.EnergyCalculations
{
    public class EnergyConsumer : CalculationObject
    {
        public Common.AbstractModel.ConsumerType Type { get; set; }
        public float Pfixed { get; set; }

        public EnergyConsumer(long globalId) : base(globalId)
        {
            
        }
    }
}
