using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labb3.Commands
{
    abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        //Stänger av knappen om det detta returnerar false
        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        //Vad som händer när man trycker på knappen
        public abstract void Execute(object? parameter);

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

    }
}
