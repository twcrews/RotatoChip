using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crews.Utility.RotatoChip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public event RoutedEventHandler PerMonitorToggled;

        public bool CursorAware
        {
            get => Convert.ToBoolean(PerMonitorRadio.IsChecked);
            set => PerMonitorRadio.IsChecked = value;
        }

        public string[] Monitors
        {
            get => (string[])MonitorComboBox.ItemsSource;
            set => MonitorComboBox.ItemsSource = value;
        }

        public string SelectedMonitor
        {
            get => MonitorComboBox.SelectedItem.ToString();
            set => MonitorComboBox.SelectedItem = value;
        }

        public SettingsWindow() => InitializeComponent();

        private void PerMonitorToggleChanged(object sender, RoutedEventArgs e)
        {
            MonitorComboBox.IsEnabled = Convert.ToBoolean(PerMonitorRadio.IsChecked);
            PerMonitorToggled?.Invoke(this, e);
        }
    }
}
