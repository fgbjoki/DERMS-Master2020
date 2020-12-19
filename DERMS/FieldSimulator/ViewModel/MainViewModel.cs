﻿using FieldSimulator.Commands;
using FieldSimulator.Modbus;
using FieldSimulator.Model;
using System.Collections.Generic;
using System.Windows.Input;

namespace FieldSimulator.ViewModel
{
    internal enum ViewModelEnum
    {
        CoilsViewModel,
        DiscreteInputViewModel,
        HoldingRegistersViewModel,
        InputRegistersViewModel,
        CalculationsViewModel
    }

    public class MainViewModel : BaseViewModel, IParentViewModel
    {
        private Dictionary<ViewModelEnum, BaseViewModel> viewModels;
        private BaseViewModel content;

        private PointController pointController;

        private ModbusSlave slave;

        private string label;

        public MainViewModel() : base("MainViewModel")
        {
            slave = new ModbusSlave(22222);
            pointController = new PointController(slave);
            pointController.Initialize();

            InitializeViewModels();
            InitializeCommands();    
        }

        public BaseViewModel ChildViewModel
        {
            get { return content; }
            set
            {
                SetProperty(ref content, value);
                Label = content.ViewModelLabel;
            }
        }

        public string Label
        {
            get { return label; }
            set { SetProperty(ref label, value); }
        }

        public ICommand ChangeViewModelToCoils { get; set; }
        public ICommand ChangeViewModelToDiscreteInputs { get; set; }
        public ICommand ChangeViewModelToHoldingRegisters { get; set; }
        public ICommand ChangeViewModelToInputRegisters { get; set; }
        public ICommand ChangeViewModelToCalculations { get; set; }

        public ICommand StartServerCommand { get; set; }
        public ICommand StopServerCommand { get; set; }

        private void InitializeViewModels()
        {
            viewModels = new Dictionary<ViewModelEnum, BaseViewModel>(5)
            {
                {ViewModelEnum.CoilsViewModel, new CoilsViewModel(pointController.Coils)},
                {ViewModelEnum.HoldingRegistersViewModel, new HoldingRegistersViewModel(pointController.HoldingRegisters) },
                {ViewModelEnum.InputRegistersViewModel, new InputRegistersViewModel(pointController.InputRegisters)},
                {ViewModelEnum.DiscreteInputViewModel, new DiscreteInputsViewModel(pointController.DiscreteInputs)},
                {ViewModelEnum.CalculationsViewModel, new CaluclationsViewModel()},
            };
        }

        private void InitializeCommands()
        {
            ChangeViewModelToCoils = new ChangeViewModelCommand(this, viewModels[ViewModelEnum.CoilsViewModel]);
            ChangeViewModelToDiscreteInputs = new ChangeViewModelCommand(this, viewModels[ViewModelEnum.DiscreteInputViewModel]);
            ChangeViewModelToHoldingRegisters = new ChangeViewModelCommand(this, viewModels[ViewModelEnum.HoldingRegistersViewModel]);
            ChangeViewModelToInputRegisters = new ChangeViewModelCommand(this, viewModels[ViewModelEnum.InputRegistersViewModel]);
            ChangeViewModelToCalculations = new ChangeViewModelCommand(this, viewModels[ViewModelEnum.CalculationsViewModel]);

            StopServerCommand = new SimulatorStopCommand(slave);
            StartServerCommand = new SimulatorStartCommand(slave);
        }
    }
}