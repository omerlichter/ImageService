using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    interface IVMSettingsPage : INotifyPropertyChanged
    {
        string VM_OutputDirectory { get; }
        string VM_SourceName { get; }
        string VM_LogName { get; }
        int VM_ThumbnailSize { get; }
        bool VM_ServerConnection { get; }
        ObservableCollection<string> VM_LbHandlers { get; }
    }
}
