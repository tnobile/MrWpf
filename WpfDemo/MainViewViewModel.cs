using GalaSoft.MvvmLight;
using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using WpfDemo.ViewModel;

namespace WpfDemo
{
    /**
     * http://neue.cc/2011/10/07_346.html
     */
    public class MainViewViewModel : ViewModelBase
    {
        private readonly ObservableCollection<string> _messages = new ObservableCollection<string>();
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
        }

        private readonly MyCollectionViewModel _myCollectionViewModel = new MyCollectionViewModel();
        public MyCollectionViewModel MyCollectionViewModel
        {
            get { return _myCollectionViewModel; }
        } 

        private readonly ObservableCollection<string> _messages2 = new ObservableCollection<string>();
        private ObservableCollection<string> _dualMessages;
        public ObservableCollection<string> DualMessages { get { return _dualMessages; } set { Set(ref _dualMessages, value); } }


        //public ReadOnlyReactiveCollection<PersonViewModel> People { get; private set; }

        public ReactiveProperty<string> CurrentText { get; private set; }
        public ReactiveProperty<string> DisplayText { get; private set; }

        #region commands
        public ReactiveCommand Start { get; private set; }
        public ReactiveCommand Stop { get; private set; }
        public ReactiveCommand Clear { get; private set; }
        public ReactiveCommand Switch { get; private set; }
        #endregion

        private IScheduler scheduler;
        public MainViewViewModel()
        {
            scheduler = scheduler ?? UIDispatcherScheduler.Default;

            //IsStarted = this.ObserveProperty(x => x.HasStarted).ToReactiveProperty();
            IsStarted = new ReactiveProperty<bool>(false);


            Start = new ReactiveCommand();
            Start.Subscribe(a =>
            {
                IsStarted.Value = true;
                HasStarted = true;
                Console.WriteLine("Started at " + DateTime.Now.ToLongTimeString());
            });

            
            Stop = new ReactiveCommand(IsStarted);
            Stop.Subscribe(_ => Halt());

            Ticker
                .Where(_ => !IsHalted && HasStarted)
                .ObserveOn(scheduler)
                .Subscribe(x => _messages.Add(x.ToLongTimeString()));

            Ticker
                .Where(_ => !IsHalted && HasStarted)
                .ObserveOn(scheduler)
                .Subscribe(x => _messages2.Add(x.ToString()));


            (Clear = new ReactiveCommand()).Subscribe(_ => Messages.Clear());


            CurrentText = new ReactiveProperty<string>();
            CurrentText.Value = "";

            // そして、それを元にして加工してUIへ返してみたり
            DisplayText = CurrentText
                .Select(x => x.ToUpper()) // 全て大文字にして
                .Delay(TimeSpan.FromSeconds(3)) // 3秒後に値を流す
                .ToReactiveProperty();

            DualMessages = _messages;
            Switch = new ReactiveCommand();
            Switch.Subscribe(_ =>
            {
                if (DualMessages == _messages)
                    DualMessages = _messages2;
                else
                    DualMessages = _messages;
            });

        }

        public ReactiveProperty<bool> IsStarted { get; private set; }

        private bool _hasStarted;
        public bool HasStarted
        {
            get { return _hasStarted; }
            set { Set(ref _hasStarted, value); }
        }
        public bool IsHalted { get; set; }
        public void Halt()
        {
            IsHalted = !IsHalted;
        }

        public IObservable<DateTime> Ticker
        {
            get
            {
                return Observable.Interval(TimeSpan.FromSeconds(0.2)).Select(_ => DateTime.Now);
            }
        }
    }
}
