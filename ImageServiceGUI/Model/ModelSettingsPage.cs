using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ImageServiceGUI.Client;
using System.Collections.ObjectModel;

namespace ImageServiceGUI.Model
{
    class ModelSettingsPage : IModelSettingsPage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string outputDirectory;
        private string sourceName;
        private string logName;
        private int thumbnailSize;
        private bool serverConnection;
        private ObservableCollection<string> lbHandlers = new ObservableCollection<string>();

        public string OutputDirectory {
            set {
                this.outputDirectory = value;
                this.NotifyPropertyChanged("OutputDirectory");
            }
            get { return this.outputDirectory; }
        }
        public string SourceName {
            set {
                this.sourceName = value;
                this.NotifyPropertyChanged("SourceName");
            }
            get { return this.sourceName; }
        }
        public string LogName {
            set {
                this.logName = value;
                this.NotifyPropertyChanged("LogName");
            }
            get { return this.logName; }
        }
        public int ThumbnailSize {
            set {
                this.thumbnailSize = value;
                this.NotifyPropertyChanged("ThumbnailSize");
            }
            get { return this.thumbnailSize; }
        }
        public bool ServerConnection
        {
            set {
                this.serverConnection = value;
                this.NotifyPropertyChanged("ServerConnection");
            }
            get { return this.serverConnection; }
        }
        public ObservableCollection<string> LbHandlers
        {
            get { return this.lbHandlers; }
        }

        public ModelSettingsPage()
        {
            this.lbHandlers.Add("bla bla");
            this.lbHandlers.Add("gggg gggg");
            this.lbHandlers.Add("abc abc abc");
            TCPClient client = TCPClient.Instance;
            bool a = client.StartCommunication();
            Console.WriteLine(a);
        }


        public void NotifyPropertyChanged(string propName) {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void GetSettingsFromService()
        {
            TCPClient client = TCPClient.Instance;
            string command = "1";
            client.WriteToServer(command);
        }

        public bool RemoveHandler(string handler)
        {
            TCPClient client = TCPClient.Instance;
            string command = "2";
            client.WriteToServer(command);
            return true;
        }
    }
}
