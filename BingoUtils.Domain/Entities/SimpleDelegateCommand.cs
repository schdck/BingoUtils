using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BingoUtils.Domain.Entities
{
    public class SimpleDelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<object> Command;

        public SimpleDelegateCommand(Action<object> Command)
        {
            this.Command = Command;
        }

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            Command?.Invoke(parameter);
        }
    }
}
