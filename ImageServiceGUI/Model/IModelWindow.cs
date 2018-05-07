using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface IModelWindow : INotifyPropertyChanged
    {
        bool Connect { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
