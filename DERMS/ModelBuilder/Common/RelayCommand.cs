﻿using System;
using System.Windows.Input;

namespace ClientUI.Common
{
    public class RelayCommand : ICommand
	{
		private Action<object> _execute;
		private Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
				throw new NullReferenceException("execute parameter is null!");

			_execute = execute;
			_canExecute = canExecute;
		}

		public RelayCommand(Action<object> execute) : this(execute, null)
		{

		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute.Invoke(parameter);
		}
	}
}
