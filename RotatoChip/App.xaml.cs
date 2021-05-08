using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crews.Utility.RotatoChip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SettingsWindow SettingsWindow { get; set; }
        private List<Display> Displays { get; set; }
        private KeyboardHook KeyboardHook { get; set; }
        private Keyboard Keyboard { get; set; }
        private Configuration Configuration { get; set; }
        private Shortcut SelectedShortcut { get; set; }
        private TaskbarIcon TrayIcon { get; set; }

        private Display _SelectedDisplay;
        private Display SelectedDisplay 
        {
            get => _SelectedDisplay;
            set
            {
                _SelectedDisplay = value;
                PopulateShortcuts();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(
                Process.GetCurrentProcess().MainModule.ModuleName)).Length > 1)
            {
                MessageBox.Show("Rotato Chip is already running!", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
            }
            Displays = Display.GetAllDisplays();
            CreateKeyboardMap();
            CreateKeyboardHook();
            GetConfiguration();
            CreateTrayIcon();
            if (Configuration.ShowSettingsOnStart)
            {
                CreateSettingsWindow();
            }
        }

        private void CreateKeyboardMap()
        {
            Keyboard = new();
            Keyboard.PressedKeysChanged += HandleShortcutInput;
        }

        private void CreateKeyboardHook()
        {
            KeyboardHook = new();
            KeyboardHook.KeyUp += Keyboard.HandleKeyUp;
            KeyboardHook.KeyDown += Keyboard.HandleKeyDown;
        }

        private void GetConfiguration()
        {
            Configuration = Configuration.Load();
            ConfigureShortcuts();
        }

        private void ConfigureShortcuts()
        {
            foreach (Display display in Displays)
            {
                foreach (Orientation orientation in (Orientation[])Enum.GetValues(typeof(Orientation)))
                {
                    AddEmptyShortcutIfMissing(display, orientation);
                }
            }
            Configuration.Save();
        }

        public void AddEmptyShortcutIfMissing(Display display, Orientation orientation)
        {
            Shortcut shortcut = Configuration.Shortcuts.Find(shortcut =>
                shortcut.DeviceName == display.DeviceName && shortcut.Orientation == orientation);
            if (shortcut == null)
            {
                Configuration.Shortcuts.Add(new Shortcut
                {
                    DeviceName = display.DeviceName,
                    Keys = new(),
                    Orientation = orientation
                });
            }
        }

        private void CreateTrayIcon()
        {
            TrayIcon = new();
            TrayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule.FileName);
            TrayIcon.TrayLeftMouseDown += HandleTrayIconClick;
            TrayIcon.TrayRightMouseDown += HandleTrayIconClick;
        }

        private void HandleTrayIconClick(object sender, RoutedEventArgs e)
        {
            if (SettingsWindow == null || !SettingsWindow.IsLoaded)
            {
                CreateSettingsWindow();
            }
        }
        
        private void CreateSettingsWindow()
        {
            SettingsWindow = new();
            UseWindowEvents();
            SettingsWindow.DisplayNames = Displays.Select(display => display.Name).ToArray();
            SettingsWindow.SelectedDisplayName = SettingsWindow.DisplayNames.First();
            SettingsWindow.StartWithWindows = Configuration.StartWithWindows;
            SettingsWindow.ShowSettingsOnStart = Configuration.ShowSettingsOnStart;
            SettingsWindow.Show();
        }

        private void UseWindowEvents()
        {
            SettingsWindow.DisplayChanged += HandleSelectedDisplayChanged;
            SettingsWindow.ShortcutChanging += BeginListeningForShortcut;
            SettingsWindow.ShortcutChanged += StopListeningForShortcut;
            SettingsWindow.SaveClicked += HandleSave;
            SettingsWindow.StartWithWindowsChanged += HandleStartWithWindowsChanged;
            SettingsWindow.ShowSettingsOnStartChanged += HandleShowSettingsOnStartChanged;
            SettingsWindow.Closed += HandleSettingsWindowClosed;
            SettingsWindow.Exit += HandleExit;
        }

        private void HandleSelectedDisplayChanged(object sender, EventArgs e) => 
            SelectedDisplay = Displays.Find(display => display.Name == SettingsWindow.SelectedDisplayName);

        private void BeginListeningForShortcut(object sender, ShortcutEventArgs e)
        {
            SelectedShortcut = Configuration.Shortcuts.Find(s => 
                s.DeviceName == SelectedDisplay.DeviceName && s.Orientation == e.Orientation);
            SelectedShortcut.Keys.Clear();
            GetShortcutContent(SelectedDisplay, e.Orientation);
            Keyboard.PressedKeysChanged -= HandleShortcutInput;
            Keyboard.PressedKeysChanged += HandleSetShortcut;
        }

        private void StopListeningForShortcut(object sender, ShortcutEventArgs e)
        {
            Keyboard.PressedKeysChanged -= HandleSetShortcut;
            Keyboard.PressedKeysChanged += HandleShortcutInput;
            SettingsWindow.SaveEnabled = true;
        }

        private void HandleSave(object sender, EventArgs e)
        {
            SettingsWindow.SaveEnabled = false;
            SettingsWindow.Close();
            Configuration.Save();
        }

        private void HandleStartWithWindowsChanged(object sender, EventArgs e)
        {
            Configuration.StartWithWindows = SettingsWindow.StartWithWindows;
            SettingsWindow.SaveEnabled = true;
        }

        private void HandleShowSettingsOnStartChanged(object sender, EventArgs e)
        {
            Configuration.ShowSettingsOnStart = SettingsWindow.ShowSettingsOnStart;
            SettingsWindow.SaveEnabled = true;
        }

        private void HandleSettingsWindowClosed(object sender, EventArgs e) => StopListeningForShortcut(null, null);

        private void HandleExit(object sender, EventArgs e) => Shutdown();

        private void HandleSetShortcut(object sender, EventArgs e)
        {
            Key lastKey = Keyboard.PressedKeys.LastOrDefault();
            if (OkayToAddKey(lastKey))
            {
                SelectedShortcut.Keys.Add(lastKey);
            }
            GetShortcutContent(SelectedDisplay, SelectedShortcut.Orientation);
        }

        private bool OkayToAddKey(Key key)
        {
            return key != Key.None &&
                SelectedShortcut.Keys.Count < 5 &&
                !SelectedShortcut.Keys.Contains(key);
        }

        private void PopulateShortcuts()
        {
            GetShortcutContent(SelectedDisplay, Orientation.Landscape);
            GetShortcutContent(SelectedDisplay, Orientation.Portrait);
            GetShortcutContent(SelectedDisplay, Orientation.UpsideDown);
            GetShortcutContent(SelectedDisplay, Orientation.PortraitInverted);
        }

        private void GetShortcutContent(Display display, Orientation orientation)
        {
            UIElementCollection content = orientation switch
            {
                Orientation.Landscape => SettingsWindow.LandscapeShortcutContent,
                Orientation.Portrait => SettingsWindow.PortraitShortcutContent,
                Orientation.UpsideDown => SettingsWindow.UpsideDownShortcutContent,
                Orientation.PortraitInverted => SettingsWindow.PortraitInvertedShortcutContent,
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, 
                    "Unsupported orientation.")
            };

            content.Clear();
            Shortcut shortcut = Configuration.Shortcuts.Find(s => 
                s.DeviceName == display.DeviceName && s.Orientation == orientation);
            if (shortcut != null)
            {
                foreach (Key key in shortcut.Keys)
                {
                    content.Add(new KeyControl(key));
                }
            }
            if (content.Count == 0)
            {
                content.Add(new TextBlock
                {
                    Text = "Not set",
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromRgb(120, 120, 120))
                });
            }
        }

        private void CheckForShortcut()
        {
            foreach (Shortcut shortcut in Configuration.Shortcuts)
            {
                if (shortcut.MatchesInput(Keyboard.PressedKeys.ToList())) 
                {
                    Displays.Find(display => display.DeviceName == shortcut.DeviceName).Rotate(shortcut.Orientation);
                }
            }
        }

        private void HandleShortcutInput(object sender, EventArgs e) => CheckForShortcut();
    }
}
