using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfDaiNet.Model;
using WpfDaiNet.Model.Managers;

namespace WpfDaiNet.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _imageSource;
        private bool _isEnabled;
        private int _objectsCount = 100;
        private Command generateCommand;

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                IsEnabled = _imageSource != null;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public int ObjectsCount
        {
            get => _objectsCount;
            set
            {
                _objectsCount = value;
                OnPropertyChanged();
            }
        }

        public Command GenerateCommand
        {
            get
            {
                return generateCommand ??
                  (generateCommand = new Command(obj =>
                  {
                      ImageSource = ImageManager.GenerateImage(ObjectsCount);
                  }));
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

        public void SetSettings(bool isClustering, string N, string K, string limit, string cloning, string mutation, string numbersOfClones, string cloningC, string mutationC, string delay)
        {
            AppModel.SetSettingsParameters(ImageSource, isClustering, N, K, limit, cloning, mutation, numbersOfClones, cloningC, mutationC, delay);
        }
    }
}
