namespace CalculationEngine.Commanding.Commands
{
    public class EnergyStorageIdleStateCommand : BaseEnergyStorageCommand
    {
        public override float ActivePower { get { return 0; } }
    }
}
