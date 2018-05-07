using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Client;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class VMWindow : IVMWindow
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IModelWindow m_model;

        public VMWindow()
        {
            this.m_model = new ModelWindow();
            this.m_model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public bool VM_Connect {
            get { return this.m_model.Connect; }
        }

        public bool Connection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
