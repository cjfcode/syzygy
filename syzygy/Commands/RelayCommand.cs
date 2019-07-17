using System;
using System.Windows.Input;

namespace Syzygy
{
    internal class RelayCommand : ICommand
    {
        private readonly Action action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            action();
        }
    }
}