using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    interface IVMWindow : INotifyPropertyChanged
    {
        bool Connection { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
