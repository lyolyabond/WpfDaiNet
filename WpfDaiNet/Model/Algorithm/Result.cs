using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfDaiNet.Model
{
    class Result : INotifyPropertyChanged
    {
        private double _seconds;

        public double Seconds
        {
            get => _seconds;
            set
            {
                _seconds = Math.Round(value, 5);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
