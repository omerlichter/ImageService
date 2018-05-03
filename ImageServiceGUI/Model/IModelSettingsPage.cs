using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ImageServiceGUI.Model
{
    interface IModelSettingsPage : INotifyPropertyChanged
    {
        string OutputDirectory { set; get; }
        string SourceName { set; get; }
        string LogName { set; get; }
        int ThumbnailSize { set; get; }
        bool ServerConnection { set; get; }
        ObservableCollection<string> LbHandlers { get; }

        void GetSettingsFromService();
        bool RemoveHandler(string handlerPath);
    }
}
