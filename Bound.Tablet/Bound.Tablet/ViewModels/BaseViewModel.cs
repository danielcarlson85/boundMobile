using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bound.Tablet.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string labelWeight;
        public string LabelWeight
        {
            get { return labelWeight; }
            set { SetProperty(ref labelWeight, value); }

        }

        private string labelText;
        public string LabelText
        {
            get { return labelText; }
            set { SetProperty(ref labelText, value); }
        }

        private string labelMachineName;
        public string LabelMachineName
        {
            get { return labelMachineName; }
            set { SetProperty(ref labelMachineName, value); }
        }

        private string imageNFC;
        public string ImageNFC
        {
            get { return imageNFC; }
            set { SetProperty(ref imageNFC, value); }
        }

        private string imageCurrentMachine;
        public string ImageCurrentMachine
        {
            get { return imageCurrentMachine; }
            set { SetProperty(ref imageCurrentMachine, value); }
        }

        private System.Drawing.Color labelDeviceStatus;
        public System.Drawing.Color LabelDeviceStatus
        {
            get { return labelDeviceStatus; }
            set { SetProperty(ref labelDeviceStatus, value); }
        }

        private System.Drawing.Color labelDeviceIsRunning;
        public System.Drawing.Color LabelDeviceIsRunning
        {
            get { return labelDeviceIsRunning; }
            set { SetProperty(ref labelDeviceIsRunning, value); }
        }

        private System.Drawing.Color deviceStatus;
        public System.Drawing.Color DeviceStatus
        {
            get { return deviceStatus; }
            set { SetProperty(ref deviceStatus, value); }
        }

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set => SetProperty(ref _isRefreshing, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
