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
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Communication;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Windows;

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
            TCPClient client = TCPClient.Instance;
            client.MessageReceived += GetMessageFromClient;
            this.serverConnection = client.StartCommunication();
            Console.WriteLine(this.ServerConnection);
        }

        public void NotifyPropertyChanged(string propName) {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void GetSettingsFromService()
        {
            TCPClient client = TCPClient.Instance;
            MessageInfo info = new MessageInfo(CommandEnum.GetConfigCommand, null);
            string command = JsonConvert.SerializeObject(info);
            client.WriteToServer(command);
        }

        public bool RemoveHandler(string handler)
        {
            Console.WriteLine("remove handler " + handler);
            TCPClient client = TCPClient.Instance;
            MessageInfo info = new MessageInfo(CommandEnum.CloseCommand, handler);
            string command = JsonConvert.SerializeObject(info);
            client.WriteToServer(command);
            return true;
        }

        public void GetMessageFromClient(object sender, MessageInfo info)
        {
            if (info.ID == CommandEnum.GetConfigCommand)
            {
                try
                {
                    Console.WriteLine(info.Args);
                    ConfigData configData = JsonConvert.DeserializeObject<ConfigData>(info.Args);

                    Application.Current.Dispatcher.Invoke(new Action(() => 
                    {
                        this.OutputDirectory = configData.OutputDir;
                        this.SourceName = configData.SourceName;
                        this.LogName = configData.LogName;
                        this.ThumbnailSize = configData.ThumbnailSize;
                        foreach (string handler in configData.Handlers)
                        {
                            this.lbHandlers.Add(handler);
                        }
                    }));
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (info.ID == CommandEnum.CloseCommand)
            {
                try
                {
                    string closedHandler = info.Args;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.lbHandlers.Remove(closedHandler);
                    }));              
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
