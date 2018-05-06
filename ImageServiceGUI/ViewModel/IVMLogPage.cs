using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Communication;

namespace ImageServiceGUI.ViewModel
{
    interface IVMLogPage : INotifyPropertyChanged
    {
        ObservableCollection<LogItem> VM_LogsList { get; }
    }
}
