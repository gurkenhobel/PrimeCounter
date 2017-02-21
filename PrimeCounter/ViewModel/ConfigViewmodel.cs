using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCounter.ViewModel
{
    public class ConfigViewmodel : BaseViewModel
    {
        private bool _fermat;
        private bool _mrt;
        private bool _bruteForce;

        public ConfigViewmodel()
        {
            PropertyChanged += (s, e) =>
            {
                if (!(_fermat || _mrt || _bruteForce))
                {
                    BruteForce = true;
                }
            };
        }

        public bool Fermat
        {
            get
            {
                return _fermat;
            }

            set
            {
                _fermat = value;
                OnPropertyChanged(nameof(Fermat));
            }
        }

        public bool Mrt
        {
            get
            {
                return _mrt;
            }

            set
            {
                _mrt = value;
                OnPropertyChanged(nameof(Mrt));
            }
        }

        public bool BruteForce
        {
            get
            {
                return _bruteForce;
            }

            set
            {
                _bruteForce = value;
                OnPropertyChanged(nameof(BruteForce));
            }
        }
    }
}
