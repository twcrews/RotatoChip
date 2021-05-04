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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Display.Rotate(1, Display.Orientations.DEGREES_CW_0);

            Window = new();
            Window.Closed += HandleWindowClose;
            Window.Monitors = new string[] { "Test1", "Test2" };
            Window.SelectedMonitor = Window.Monitors.First();
            Window.CursorAware = true;
            Window.Show();
        }

        private void HandleWindowClose(object sender, EventArgs e) => OnExit(null);
    }
}
