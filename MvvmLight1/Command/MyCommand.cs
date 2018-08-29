﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmLight1.Command
{
    public class MyCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public MyCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            this.execute = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
