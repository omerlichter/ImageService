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

        /// <summary>
        /// cand execute the function
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if can</returns>
        public bool CanExecute(object obj)
        {
            return this.canExecuteMethod(obj);
        }

        /// <summary>
        /// execute the function
        /// </summary>
        /// <param name="obj"></param>
        public void Execute(object obj)
        {
            this.executeMethod(obj);
        }

        /// <summary>
        /// raise event
        /// </summary>
        public void RaiseCanExecuteChange()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
