using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainApp.Command
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

#pragma warning disable CS0067 // Событие "MyCommand.CanExecuteChanged" никогда не используется.
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // Событие "MyCommand.CanExecuteChanged" никогда не используется.

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
