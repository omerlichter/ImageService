using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Client;
using ImageServiceGUI.Model;
using System.Windows.Input;
using ImageServiceGUI.CommandBinding;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Communication;
using Newtonsoft.Json;

namespace ImageServiceGUI.ViewModel
{
    class VMWindow : IVMWindow
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IModelWindow m_model;

        public ICommand CloseWindowCommand { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        public VMWindow()
        {
            this.m_model = new ModelWindow();
            this.m_model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.CloseWindowCommand = new DelegateCommand<object>(this.OnCloseWindow, this.CanCloseWindow);
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

        /// <summary>
        /// notify that property changed
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// when close button press
        /// </summary>
        /// <param name="obj"></param>
        private void OnCloseWindow(object obj)
        {
            // sned server that the client is close
            TCPClient client = TCPClient.Instance;
            MessageInfo info = new MessageInfo(CommandEnum.CloseGUICommand, null);
            string command = JsonConvert.SerializeObject(info);
            client.WriteToServer(command);
            TCPClient.Instance.CloseCommunication();
        }

        /// <summary>
        /// if can close window
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanCloseWindow(object obj)
        {
            return true;
        }
    }
}
