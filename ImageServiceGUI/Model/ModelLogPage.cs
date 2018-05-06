using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.ViewModel;
using ImageServiceGUI.Client;
using ImageService.Infrastructure.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System.Windows;

namespace ImageServiceGUI.Model
{
    class ModelLogPage : IModelLogPage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<LogItem> logsList = new ObservableCollection<LogItem>();

        public ObservableCollection<LogItem> LogsList {
            get { return this.logsList; }
            set
            {
                this.logsList = value;
                this.NotifyPropertyChanged("LogsList");
            }
        }

        public ModelLogPage()
        {
            TCPClient client = TCPClient.Instance;
            client.MessageReceived += GetMessageFromClient;
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void GetLogsHistoryFromService()
        {
            TCPClient client = TCPClient.Instance;
            MessageInfo info = new MessageInfo(CommandEnum.LogCommand, null);
            string command = JsonConvert.SerializeObject(info);
            client.WriteToServer(command);
        }

        public void GetMessageFromClient(object sender, MessageInfo info)
        {
            if (info.ID == CommandEnum.LogCommand)
            {
                try
                {
                    LogData logData = JsonConvert.DeserializeObject<LogData>(info.Args);
                    foreach(LogItem log in logData.LogsList)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            this.LogsList.Add(log);
                        }));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
