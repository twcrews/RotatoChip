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
        public string[] Displays
        {
            get => (string[])DisplayComboBox.ItemsSource;
            set => DisplayComboBox.ItemsSource = value;
        }

        public string SelectedDisplay
        {
            get => DisplayComboBox.SelectedItem.ToString();
            set => DisplayComboBox.SelectedItem = value;
        }

        public SettingsWindow() => InitializeComponent();

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
