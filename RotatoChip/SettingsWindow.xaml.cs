using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Crews.Utility.RotatoChip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public delegate void ShortcutEventHandler(object sender, ShortcutEventArgs e);

        public event EventHandler DisplayChanged;
        public event ShortcutEventHandler ShortcutChanging;
        public event ShortcutEventHandler ShortcutChanged;
        public event EventHandler SaveClicked;
        public event EventHandler StartWithWindowsChanged;
        public event EventHandler ShowSettingsOnStartChanged;
        public event EventHandler Exit;

        public string[] DisplayNames
        {
            get => (string[])DisplayComboBox.ItemsSource;
            set => DisplayComboBox.ItemsSource = value;
        }

        public string SelectedDisplayName
        {
            get => DisplayComboBox.SelectedItem.ToString();
            set => DisplayComboBox.SelectedItem = value;
        }

        public bool SaveEnabled
        {
            get => SaveButton.IsEnabled;
            set => SaveButton.IsEnabled = value;
        }

        public bool StartWithWindows
        {
            get => (bool)StartWithWindowsToggle.IsChecked;
            set => StartWithWindowsToggle.IsChecked = value;
        }

        public bool ShowSettingsOnStart
        {
            get => (bool)ShowSettingsOnStartToggle.IsChecked;
            set => ShowSettingsOnStartToggle.IsChecked = value;
        }

        public UIElementCollection LandscapeShortcutContent => LandscapeShortcutStackPanel.Children;
        public UIElementCollection PortraitShortcutContent => PortraitShortcutStackPanel.Children;
        public UIElementCollection UpsideDownShortcutContent => UpsideDownShortcutStackPanel.Children;
        public UIElementCollection PortraitInvertedShortcutContent => PortraitInvertedShortcutStackPanel.Children;

        public SettingsWindow() => InitializeComponent();

        private const string BUTTON_SET_CONTENT = "SET";
        private const string BUTTON_DONE_CONTENT = "DONE";

        private bool LandscapeSetting
        {
            get => LandscapeShortcutSetButton.Content.ToString() != BUTTON_SET_CONTENT;
            set => ButtonSetting(Orientation.Landscape, value);
        }

        private bool PortraitSetting
        {
            get => PortraitShortcutSetButton.Content.ToString() != BUTTON_SET_CONTENT;
            set => ButtonSetting(Orientation.Portrait, value);
        }

        private bool UpsideDownSetting
        {
            get => UpsideDownShortcutSetButton.Content.ToString() != BUTTON_SET_CONTENT;
            set => ButtonSetting(Orientation.UpsideDown, value);
        }

        private bool PortraitInvertedSetting
        {
            get => PortraitInvertedShortcutSetButton.Content.ToString() != BUTTON_SET_CONTENT;
            set => ButtonSetting(Orientation.PortraitInverted, value);
        }

        private void ButtonSetting(Orientation orientation, bool setting)
        {
            ChangeButtonsEnabledState(!setting);
            if (orientation == Orientation.Landscape)
            {
                LandscapeShortcutSetButton.Content = setting ? BUTTON_DONE_CONTENT : BUTTON_SET_CONTENT;
                LandscapeShortcutSetButton.IsEnabled = true;
            }
            if (orientation == Orientation.Portrait)
            {
                PortraitShortcutSetButton.Content = setting ? BUTTON_DONE_CONTENT : BUTTON_SET_CONTENT;
                PortraitShortcutSetButton.IsEnabled = true;
            }
            if (orientation == Orientation.UpsideDown)
            {
                UpsideDownShortcutSetButton.Content = setting ? BUTTON_DONE_CONTENT : BUTTON_SET_CONTENT;
                UpsideDownShortcutSetButton.IsEnabled = true;
            }
            if (orientation == Orientation.PortraitInverted)
            {
                PortraitInvertedShortcutSetButton.Content = setting ? BUTTON_DONE_CONTENT : BUTTON_SET_CONTENT;
                PortraitInvertedShortcutSetButton.IsEnabled = true;
            }

            if (setting)
            {
                ShortcutChanging(this, new ShortcutEventArgs(orientation));
            }
            else
            {
                ShortcutChanged(this, new ShortcutEventArgs(orientation));
            }
        }

        private void ChangeButtonsEnabledState(bool state)
        {
            foreach (Button button in new Button[] 
                { 
                    LandscapeShortcutSetButton, 
                    PortraitShortcutSetButton, 
                    UpsideDownShortcutSetButton, 
                    PortraitInvertedShortcutSetButton 
                })
            {
                button.IsEnabled = state;
            }
        }

        private void SetShortcutButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == LandscapeShortcutSetButton)
            {
                LandscapeSetting = !LandscapeSetting;
            }
            if (button == PortraitShortcutSetButton)
            {
                PortraitSetting = !PortraitSetting;
            }
            if (button == UpsideDownShortcutSetButton)
            {
                UpsideDownSetting = !UpsideDownSetting;
            }
            if (button == PortraitInvertedShortcutSetButton)
            {
                PortraitInvertedSetting = !PortraitInvertedSetting;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();
        private void SaveButton_Click(object sender, RoutedEventArgs e) => SaveClicked(sender, e);

        private void DisplayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            DisplayChanged(sender, e);

        private void StartWithWindowsToggle_Click(object sender, RoutedEventArgs e) => 
            StartWithWindowsChanged(sender, e);

        private void ShowSettingsOnStartToggle_Click(object sender, RoutedEventArgs e) => 
            ShowSettingsOnStartChanged(sender, e);

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Exit(sender, e);

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e) => e.Handled = true;
    }

    public class ShortcutEventArgs : EventArgs
    {
        public Orientation Orientation;

        public ShortcutEventArgs(Orientation orientation) => Orientation = orientation;
    }
}
