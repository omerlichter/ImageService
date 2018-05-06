using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using System.ComponentModel;
using ImageServiceGUI.ViewModel;
using ImageService.Infrastructure.Communication;

namespace ImageServiceGUI.Model
{
    interface IModelLogPage : INotifyPropertyChanged
    {
        ObservableCollection<LogItem> LogsList { get; set; }
    }
}
