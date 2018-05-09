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

        /// <summary>
        /// get settings from service
        /// </summary>
        void GetSettingsFromService();

        /// <summary>
        /// remove handler
        /// </summary>
        /// <param name="handlerPath"></param>
        /// <returns>true if remove</returns>
        bool RemoveHandler(string handlerPath);
    }
}
