﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageServiceGUI.Model;
using System.ComponentModel;
using ImageService.Infrastructure.Communication;

namespace ImageServiceGUI.ViewModel
{
    class VMLogPage : IVMLogPage
    {
        private IModelLogPage model;
        private LogItem selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<LogItem> VM_LogsList
        {
            get { return this.model.LogsList; }
        }

        public LogItem SelectedItem
        {
            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get { return this.selectedItem; }
        }

        public VMLogPage(IModelLogPage model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}