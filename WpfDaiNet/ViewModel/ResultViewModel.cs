using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DaiNet;
using WpfDaiNet.Model;
using WpfDaiNet.Model.Managers;
using System.Windows;
using System.Linq;

namespace WpfDaiNet.ViewModel
{
    class ResultViewModel : INotifyPropertyChanged
    {
        private bool _isClustering;
        private int _N;
        private int _K;
        private int _limit;
        private string _cloningType;
        private string _mutationType;
        private int _delay;
        private string _imagePath;
        private int _objectsNumber;
        private double _time;
        private Command startCommand;

        public bool IsClustering
        {
            get => _isClustering;
            set
            {
                _isClustering = value;
                OnPropertyChanged();
            }
        }

        public bool IsSimulation
        {
            get => !_isClustering;
            set
            {
                _isClustering = !value;
                OnPropertyChanged("IsClustering");
            }
        }

        public int N
        {
            get => _N;
            set
            {
                _N = value;
                OnPropertyChanged();
            }
        }

        public int K
        {
            get => _K;
            set
            {
                _K = value;
                OnPropertyChanged();
            }
        }

        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                OnPropertyChanged();
            }
        }

        public string CloningType
        {
            get => _cloningType;
            set
            {
                _cloningType = value;
                OnPropertyChanged();
            }
        }

        public string MutationType
        {
            get => _mutationType;
            set
            {
                _mutationType = value;
                OnPropertyChanged();
            }
        }

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                OnPropertyChanged();
            }
        }

        private bool _isAllowedStart;
        public bool IsAllowedStart
        {
            get => _isAllowedStart;
            private set
            {
                _isAllowedStart = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            private set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public int ObjectsNumber
        {
            get => _objectsNumber;
            set
            {
                _objectsNumber = value;
                OnPropertyChanged();
            }
        }

        public double Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public Command StartCommand
        {
            get
            {
                return startCommand ??
                  (startCommand = new Command(obj =>
                  {
                      SetSettings();
                      Start();
                  }));
            }
        }


        public ResultViewModel()
        {
            AppModel.Settings.PropertyChanged += OnPropertyChangedSettings;
            AppModel.Result.PropertyChanged += OnPropertyChangedResult;
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

        public async void Start()
        {
            IsAllowedStart = false;
            ImagePath = AppModel.Settings.ImagePath;
            var points = ImageManager.GetPoints(ImagePath);
            ObjectsNumber = points.Count();
            Time = 0;

            await Task.Run(async () =>
            {
                await DaiNetClustering.ClusterAsync(points, imagePath =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ImagePath = imagePath;
                    });
                });
            });

            IsAllowedStart = true;
        }

        private void SetSettings()
        {
            AppModel.SetSettingsParameters(IsClustering, N, K, Limit, CloningType, MutationType, Delay);
        }

        public void OnPropertyChangedSettings(object sender, PropertyChangedEventArgs args)
        {
            if (!(sender is Settings))
                return;
            var settings = sender as Settings;
            switch (args.PropertyName)
            {
                case "IsClustering":
                    IsClustering = settings.IsClustering;
                    break;
                case "ImagePath":
                    ImagePath = settings.ImagePath;
                    break;
                case "N":
                    N = settings.N;
                    break;
                case "K":
                    K = settings.K;
                    break;
                case "Limit":
                    Limit = settings.Limit;
                    break;
                case "CloningType":
                    CloningType = Settings.GetCloningTypeName(settings.CloningType);
                    break;
                case "MutationType":
                    MutationType = Settings.GetMutationTypeName(settings.MutationType);
                    break;
                case "Delay":
                    Delay = settings.Delay;
                    break;
            }
        }

        public void OnPropertyChangedResult(object sender, PropertyChangedEventArgs args)
        {
            if (!(sender is Model.Result))
                return;
            var result = sender as Model.Result;
            switch (args.PropertyName)
            {
                case "Seconds":
                    Time = result.Seconds;
                    break;
            }
        }
    }
}
