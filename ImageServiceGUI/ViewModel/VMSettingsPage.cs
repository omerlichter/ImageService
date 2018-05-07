using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using ImageServiceGUI.CommandBinding;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class VMSettingsPage : IVMSettingsPage
    {
        private IModelSettingsPage model;

        public event PropertyChangedEventHandler PropertyChanged;
        private string selectedItem;

        public ICommand RemoveCommand { get; private set; }

        public bool VM_ServerConnection
        {
            get {  return this.model.ServerConnection; }
        }

        public string VM_OutputDirectory
        {
            get { return this.model.OutputDirectory; }
        }

        public string VM_SourceName
        {
            get { return this.model.SourceName; }
        }

        public string VM_LogName
        {
            get { return this.model.LogName; }
        }

        public int VM_ThumbnailSize
        {
            get { return this.model.ThumbnailSize; }
        }

        public ObservableCollection<string> VM_LbHandlers
        {
            get { return this.model.LbHandlers; }
        }

        public string SelectedItem
        {
            set {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get { return this.selectedItem; }
        }

        public VMSettingsPage()
        {
            this.model = new ModelSettingsPage();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.model.GetSettingsFromService();
        }

        private void OnRemove(object obj)
        {
            // send to server remove from handlers
            this.model.RemoveHandler(this.selectedItem);
        }

        private bool CanRemove(object obj)
        {
            if (selectedItem != null)
            {
                return true;
            }
            return false;
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChange();
        }
    }
}
