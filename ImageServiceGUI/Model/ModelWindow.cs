using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Client;

namespace ImageServiceGUI.Model
{
    class ModelWindow : IModelWindow
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool connect;
        public bool Connect
        {
            get { return connect; }
            set
            {
                connect = value;
                NotifyPropertyChanged("Connect");
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ModelWindow()
        {
            connect = TCPClient.Instance.Connect;
        }

        /// <summary>
        /// notify that property chnaged
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
