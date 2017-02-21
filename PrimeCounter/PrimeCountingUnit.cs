using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PrimeCounter.Helper;
using PrimeCounter.Model;
using PrimeCounter.ViewModel;

namespace PrimeCounter
{
    class PrimeCountingUnit
    {
        private readonly int _x;
        private ObservableCollection<PrimeCandidate> _log;
        private readonly ConfigViewmodel _config;


        private List<PrimeCandidate> _remaningCandidates;
        private List<PrimeCandidate> _allCandidates;

        private CancellationTokenSource _shutdownTokenSource;
        private int _progress;

        public event EventHandler<bool> OnComplete;
        public event EventHandler OnProgress;

        private bool AllWorkDestributed => _remaningCandidates.Count == 0;



        public int Progress
        {
            get { return _progress; }
            private set
            {
                _progress = value;
                OnProgress?.Invoke(this, null);
            }
        }

        public PrimeCountingUnit(ObservableCollection<PrimeCandidate> log, ConfigViewmodel config, int x)
        {
            Progress = 0;
            _x = x;
            _shutdownTokenSource = new CancellationTokenSource();
            _log = log;
            _config = config;

            _allCandidates = PrimeCandidate.GenerateCandidateList(x - 1);
            _remaningCandidates = new List<PrimeCandidate>(_allCandidates);
        }

        public void Stop()
        {
            _shutdownTokenSource.Cancel();
        }

        public async Task Count(int counterTaskCount)
        {

            var counterTasks = new List<Task>();
            while (!AllWorkDestributed)
            {
                try
                {
                    _shutdownTokenSource.Token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    OnComplete?.Invoke(this, false);
                    return;
                }
                if (counterTasks.Count < counterTaskCount)
                {
                    var candidate = _remaningCandidates[0];
                    _remaningCandidates.Remove(candidate);
                    Application.Current.Dispatcher.Invoke(() => _log.Insert(0, candidate));

                    var newTask = CheckCandidate(candidate, _shutdownTokenSource.Token);
                    lock (counterTasks)
                        counterTasks.Add(newTask);
                    newTask.ContinueWith(t =>
                    {
                        Progress = CalculateProgress();
                        lock (counterTasks)
                        {
                            counterTasks.Remove(newTask);
                        }
                    });
                }
                else
                {
                    await Task.Delay(50);
                }
            }

            while (_allCandidates.Exists(c => !c.Complete))
                await Task.Delay(100);
            OnComplete?.Invoke(this, !_shutdownTokenSource.IsCancellationRequested);

        }

        private int CalculateProgress()
        {
            var tasksCompleted = (float)_allCandidates.Count(c => c.Complete);
            var totalTasks = (float)_allCandidates.Count;
            if (tasksCompleted < 1)
                return 0;
            return (int)(tasksCompleted / totalTasks * 100.0f);
        }

        //TODO: add more prime tests
        private async Task CheckCandidate(PrimeCandidate candidate, CancellationToken token)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (_config.Mrt)
                    {
                        candidate.MRT = PrimeCheckingUtil.MRT(candidate.Value, 20)
                            ? TestResult.Passed
                            : TestResult.Failed;
                        
                    }
                    else
                    {
                        candidate.MRT = TestResult.Skipped;
                    }
                    if (token.IsCancellationRequested)
                        return;
                    if (_config.Fermat)
                    {
                        candidate.FermatTest = PrimeCheckingUtil.FermatTest(candidate.Value, 10)
                       ? TestResult.Passed
                       : TestResult.Failed;
                        
                    }
                    else
                    {
                        candidate.FermatTest = TestResult.Skipped;
                    }
                    if (token.IsCancellationRequested)
                        return;
                    if (_config.BruteForce)
                    {
                        candidate.BruteForce = PrimeCheckingUtil.BruteForce(candidate.Value)
                        ? TestResult.Passed
                        : TestResult.Failed;
                    }
                    else
                    {
                        candidate.BruteForce = TestResult.Skipped;
                    }

                    candidate.Complete = true;
                });
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine($"Primality checking task <{candidate.Value}> canceled");
            }


        }

    }
}
