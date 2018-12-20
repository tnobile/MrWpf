using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfLib
{
    /***
     * http://lukaszlawicki.pl/observableproperty/
     */
    public class ObservableProperty<T> : object, INotifyPropertyChanged
    {
        private T value;
        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;
                DataChanged?.Invoke(this, new DataEventArgs
                {
                    Parameter = value
                });
                OnPropertyChanged(nameof(Value));
            }
        }
        public event EventHandler DataChanged;
        public ObservableProperty()
        {
            Value = default(T);
        }
        public ObservableProperty(T initValue)
        {
            Value = initValue;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }
            changed(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataEventArgs : EventArgs
    {
        public object Parameter { get; set; }
        public DataEventArgs()
        {

        }
        public DataEventArgs(object parameter)
        {
            Parameter = parameter;
        }
    }
}
