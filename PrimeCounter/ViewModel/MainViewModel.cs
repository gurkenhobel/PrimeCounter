using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PrimeCounter.Helper;
using PrimeCounter.Model;
using PrimeCounter.View;
using MessageBox = PrimeCounter.View.MessageBox;

namespace PrimeCounter.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        private int _x;
        private int _progress;
        private string _timerString;
        private ObservableCollection<PrimeCandidate> _log;

        private bool _acceptInput;
        private bool _isBackground;

        private PrimeCountingUnit _primeCounter;
        private ConfigViewmodel _config;

        public ICommand StartCommand { get; private set; }

        public int X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public int Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public ObservableCollection<PrimeCandidate> Log
        {
            get
            {
                return _log;
            }

            set
            {
                _log = value;
                OnPropertyChanged(nameof(Log));

            }
        }

        public bool AcceptInput
        {
            get
            {
                return _acceptInput;
            }

            set
            {
                _acceptInput = value;
                OnPropertyChanged(nameof(AcceptInput));

            }
        }

        public ICommand StopCommand { get; private set; }

        public bool IsBackground
        {
            get
            {
                return _isBackground;
            }

            set
            {
                _isBackground = value;
                OnPropertyChanged(nameof(IsBackground));
            }
        }

        public string TimerString
        {
            get
            {
                return _timerString;
            }

            set
            {
                _timerString = value;
                OnPropertyChanged(nameof(TimerString));
            }
        }

        public MainViewModel()
        {
            Init();
        }

        private void Init()
        {
            StartCommand = new RelayCommand((p) => Start());
            StopCommand = new RelayCommand(p =>
            {
                _primeCounter.Stop();
                AcceptInput = true;
            });

            Progress = 0;
            X = 10;
            AcceptInput = false;
            IsBackground = true;
            TimerString = "00:00";

            Application.Current.MainWindow.Loaded += (s, e) =>
            {
                _config = new ConfigViewmodel { BruteForce = true };
                new ConfigWindow(_config) {Owner = Application.Current.MainWindow}.ShowDialog();
                AcceptInput = true;
                IsBackground = false;
            };
        }

        private void Start()
        {
            Progress = 0;
            Log = new ObservableCollection<PrimeCandidate>();
            _primeCounter = new PrimeCountingUnit(Log, _config, X);
            var startTimestamp = DateTime.Now;
            _primeCounter.OnComplete += (s, e) =>
            {
                var calculationTime = DateTime.Now - startTimestamp;
                var countedPrimes = Log.Count(c => c.IsPrime);
                string msg;
                if (e)
                {
                    msg = $"Counted Primes: {countedPrimes}\ncalculation time: {new DateTime(calculationTime.Ticks):mm:ss} min";
                }
                else
                {
                    msg = $"Calculation aborted.\nCounted Primes: {countedPrimes}\ncalculation time: {new DateTime(calculationTime.Ticks):mm:ss} min";
                }

                IsBackground = true;
                var mbv = new MessageBox(msg, "Result", Application.Current.MainWindow);
                mbv.ShowDialog();
                IsBackground = false;
                
                

                AcceptInput = true;
            };
            _primeCounter.OnProgress += (s, e) =>
            {
                Progress = _primeCounter.Progress;
                TimerString = $"{new DateTime((DateTime.Now - startTimestamp).Ticks):mm:ss}";
            };
            
            _primeCounter.Count(Environment.ProcessorCount + 2);
            AcceptInput = false;
        }
    }
}
