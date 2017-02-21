using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCounter.Model
{
    public class PrimeCandidate : BaseViewModel
    {
        private TestResult _fermatTest;
        private TestResult _bruteForce;
        private TestResult _mrt;
        private bool _complete;
        private bool _isPrime;

        public PrimeCandidate(int value)
        {
            Value = value;
            Complete = false;
            FermatTest = TestResult.Pending;
            BruteForce = TestResult.Pending;
            MRT = TestResult.Pending;
        }

        public int Value { get; }

        public TestResult FermatTest
        {
            get { return _fermatTest; }
            set
            {
                _fermatTest = value;
                OnPropertyChanged(nameof(FermatTest));
            }
        }

        public TestResult BruteForce
        {
            get { return _bruteForce; }
            set
            {
                _bruteForce = value;
                OnPropertyChanged(nameof(BruteForce));
            }
        }

        public bool Complete
        {
            get { return _complete; }
            set
            {
                _complete = value;
                IsPrime = (((FermatTest | MRT | BruteForce) & TestResult.Failed) != TestResult.Failed) && value;
            }
        }

        public TestResult MRT
        {
            get
            {
                return _mrt;
            }

            set
            {
                _mrt = value;
                OnPropertyChanged(nameof(MRT));
            }
        }

        public bool IsPrime
        {
            get { return _isPrime; }
            set
            {
                _isPrime = value;
                OnPropertyChanged(nameof(IsPrime));
            }
        }

        #region static

        /// <summary>
        ///     creates a list of potential prime numbers. filters out even numbers
        /// </summary>
        /// <param name="end">biggest candidate</param>
        /// <returns></returns>
        public static List<PrimeCandidate> GenerateCandidateList(int end)
        {
            var result = new List<PrimeCandidate> ();
            if (end >= 1)
            {
                result.Add(new PrimeCandidate(1));
                if(end >= 2)
                    result.Add(new PrimeCandidate(2));
            }
            for (var i = 3; i <= end; i += 2)
            {
                result.Add(new PrimeCandidate(i));
            }
            return result;
        }
        #endregion
    }
}
