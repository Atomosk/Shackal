using System;
using System.Windows.Input;

namespace Shackal.Gui
{
    public class Command : ICommand
    {
        private Action<object> Execute { get; set; }
        private Func<object, bool> CanExecute { get; set; }

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            Execute = execute;
            CanExecute = canExecute;
        }


        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var canExecute = CanExecuteChanged;
            canExecute?.Invoke(this, EventArgs.Empty);
        }
    }
}
