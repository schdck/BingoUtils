using System;
using System.Windows.Input;

namespace BingoUtils.Domain.Entities
{
    public class SimpleDelegateCommand : ICommand
    {
        #pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
        #pragma warning restore CS0067

        private Action _Command;

        public SimpleDelegateCommand(Action command)
        {
            _Command = command;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter = null)
        {
            _Command?.Invoke();
        }
    }
}
