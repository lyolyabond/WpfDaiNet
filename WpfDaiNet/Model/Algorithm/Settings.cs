using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfDaiNet.Model
{
    class Settings : INotifyPropertyChanged
    {
        public bool _isClustering;
        private int _imageWidth;
        private int _imageHeight;
        private int _N;
        private int _K;
        private int _limit;
        private CloningType _cloningType;
        private int _cloningC;
        private int _numberOfClones;
        private MutationType _mutationType;
        private double _mutationC;

        public bool IsClustering
        {
            get => _isClustering;
            set
            {
                _isClustering = value;
                OnPropertyChanged();
            }
        }

        public Settings SetIsClustering(bool isClustering)
        {
            IsClustering = isClustering;
            return this;
        }

        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                _imageWidth = value;
                OnPropertyChanged();
            }
        }

        public Settings SetImageWidth(int imageWidth)
        {
            ImageWidth = imageWidth;
            return this;
        }

        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                _imageHeight = value;
                OnPropertyChanged();
            }
        }

        public Settings SeImageHeight(int imageHeight)
        {
            ImageHeight = imageHeight;
            return this;
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

        public Settings SetN(int n)
        {
            N = n;
            return this;
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

        public Settings SetK(int k)
        {
            K = k;
            return this;
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
        public Settings SetLimit(int limit)
        {
            Limit = limit;
            return this;
        }

        public CloningType CloningType
        {
            get => _cloningType;
            set
            {
                _cloningType = value;
                OnPropertyChanged();
            }
        }

        public Settings SetCloningType(CloningType cloningType)
        {
            CloningType = cloningType;
            return this;
        }

        public int CloningC
        {
            get => _cloningC;
            set
            {
                _cloningC = value;
                OnPropertyChanged();
            }
        }

        public Settings SetCloningC(int cloningC)
        {
            CloningC = cloningC;
            return this;
        }

        public int NumberOfClones
        {
            get => _numberOfClones;
            set
            {
                _numberOfClones = value;
                OnPropertyChanged();
            }
        }

        public Settings SetNumberOfClones(int numberOfClones)
        {
            NumberOfClones = numberOfClones;
            return this;
        }

        public MutationType MutationType
        {
            get => _mutationType;
            set
            {
                _mutationType = value;
                OnPropertyChanged();
            }
        }

        public Settings SetMutationType(MutationType mutationType)
        {
            MutationType = mutationType;
            return this;
        }

        public double MutationC
        {
            get => _mutationC;
            set
            {
                _mutationC = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImagePath"));
            }
        }

        public Settings SetMutationC(double mutationC)
        {
            MutationC = mutationC;
            return this;
        }

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                var path = Path.Combine(StaticData.FolderPath, value);
                _imagePath = Path.GetFullPath(path);
                OnPropertyChanged();
            }
        }

        public Settings SetImagePath(string imagePath)
        {
            ImagePath = imagePath;
            return this;
        }

        private int _delay;

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                OnPropertyChanged();
            }
        }

        public Settings SetDelay(int delay)
        {
            Delay = delay;
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static CloningType GetCloningType(string name)
        {
           return StaticData.CloningTypeNames.First(item => item.Value == name).Key;
        }

        public static string GetCloningTypeName(CloningType cloningType)
        {
            return StaticData.CloningTypeNames[cloningType];
        }

        public static MutationType GetMutationType(string name)
        {
            return StaticData.MutationTypeNames.First(item => item.Value == name).Key;
        }

        public static string GetMutationTypeName(MutationType mutationType)
        {
            return StaticData.MutationTypeNames[mutationType];
        }
    }
}
