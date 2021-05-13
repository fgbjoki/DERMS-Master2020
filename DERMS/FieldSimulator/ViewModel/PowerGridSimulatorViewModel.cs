using FieldSimulator.Commands.PowerSimulator;
using FieldSimulator.Commands.PowerSimulator.State;
using FieldSimulator.Modbus.SchemaAligner;
using FieldSimulator.PowerSimulator;
using System.Windows.Input;

namespace FieldSimulator.ViewModel
{
    public class PowerGridSimulatorViewModel : BaseViewModel, IPowerGridSimulatorViewModel
    {
        private string filePath;
        private PowerGridSimulatorState currentState;

        public PowerGridSimulatorViewModel(IPowerSimulator powerSimulator) : base("Power Grid Simulator")
        {
            CommandOpenFileDialog = new OpenFileDialogCommand(this);
            LoadSchemaCommand = new LoadSchemaCommand(this);
            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);

            PowerGridSimulatorState = new IdlePowerGridSimulatorState(this, powerSimulator);
        }

        public ICommand CommandOpenFileDialog { get; set; }

        public ICommand LoadSchemaCommand { get; set; }

        public ICommand StartSimulationCommand { get; set; }

        public ICommand StopSimulationCommand { get; set; }

        public PowerGridSimulatorState PowerGridSimulatorState
        {
            get { return currentState; }
            set { SetProperty(ref currentState, value); }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                if (filePath != value)
                {
                    SetProperty(ref filePath, value);
                }
            }
        }

        public void ChangeSimulatorState(PowerGridSimulatorState newState)
        {
            PowerGridSimulatorState = newState;
        }
    }
}
