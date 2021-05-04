using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Crews.Utility.RotatoChip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SettingsWindow Window { get; set; }
        private List<Display> Displays { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Displays = Display.GetAllDisplays();

            Window = new();
            Window.Closed += HandleWindowClose;
            Window.Displays = Displays.Select(display => display.Name).ToArray();
            Window.SelectedDisplay = Window.Displays.First();
            Window.Show();
        }

        private void HandleWindowClose(object sender, EventArgs e) => OnExit(null);
    }
}
