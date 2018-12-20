using MahApps.Metro.Controls;
using System;
using WpfLib;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewViewModel vm;
        private PropertyChangeNotifier notifier;
        public MainWindow()
        {
            InitializeComponent();

            notifier = new PropertyChangeNotifier(mylistview, "ItemsSource");
            notifier.ValueChanged += new EventHandler(OnValueChanged);

            vm = new MainViewViewModel();
            DataContext = vm;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ItemsSouce changed");
        }

        //private void Switch_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    mylistview.IsEnabled = false;
        //}
    }
}
