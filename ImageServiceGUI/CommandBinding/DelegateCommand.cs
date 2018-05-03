using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.CommandBinding
{
    class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public delegate void ExcuteMethod<T>(object obj);
        public delegate bool CanExecuteMethod<T>(object obj);

        private ExcuteMethod<T> executeMethod;
        private CanExecuteMethod<T> canExecuteMethod;

        public DelegateCommand(ExcuteMethod<T> executeMethod, CanExecuteMethod<T> canExecuteMethod) {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object obj)
        {
            return this.canExecuteMethod(obj);
        }

        public void Execute(object obj)
        {
            this.executeMethod(obj);
        }

        public void RaiseCanExecuteChange()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
