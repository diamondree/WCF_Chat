﻿using System;
using System.Windows.Input;

namespace WPF_Server.Commands
{
    public class DelegateCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove {  CommandManager.RequerySuggested -= value;}
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }
    }
}
