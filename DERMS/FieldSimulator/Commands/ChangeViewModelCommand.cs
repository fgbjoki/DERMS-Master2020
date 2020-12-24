using FieldSimulator.ViewModel;
using System;
using System.Windows.Input;

namespace FieldSimulator.Commands
{
    class ChangeViewModelCommand : ICommand
    {
        private IParentViewModel parentViewModel;
        private BaseViewModel childViewModel;

        public ChangeViewModelCommand(IParentViewModel parentViewModel, BaseViewModel childViewModel)
        {
            this.parentViewModel = parentViewModel;
            this.childViewModel = childViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            parentViewModel.ChildViewModel = childViewModel;
        }
    }
}
