using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Crews.Utility.RotatoChip
{
    /// <summary>
    /// Interaction logic for Key.xaml
    /// </summary>
    public partial class KeyControl : UserControl
    {
        public KeyControl(Key key)
        {
            InitializeComponent();
            try
            {
                KeyTextBlock.Text = FriendlyName[key];
            }
            catch (KeyNotFoundException)
            {
                KeyTextBlock.Text = key.ToString().ToUpperInvariant();
            }
        }

        private static readonly Dictionary<Key, string> FriendlyName = new()
        {
            { Key.Back, "⇤" },
            { Key.Clear, "CLR" },
            { Key.CapsLock, "CAPS" },
            { Key.Escape, "ESC" },
            { Key.PageUp, "PGUP" },
            { Key.PageDown, "PGDN" },
            { Key.Left, "🠔" },
            { Key.Up, "🠕" },
            { Key.Right, "🠖" },
            { Key.Down, "🠗" },
            { Key.Select, "SEL" },
            { Key.Print, "PRT" },
            { Key.PrintScreen, "PRTSCN" },
            { Key.Insert, "INS" },
            { Key.Delete, "DEL" },
            { Key.NumPad0, "N0" },
            { Key.NumPad1, "N1" },
            { Key.NumPad2, "N2" },
            { Key.NumPad3, "N3" },
            { Key.NumPad4, "N4" },
            { Key.NumPad5, "N5" },
            { Key.NumPad6, "N6" },
            { Key.NumPad7, "N7" },
            { Key.NumPad8, "N8" },
            { Key.NumPad9, "N9" },
            { Key.Multiply, "N*" },
            { Key.Add, "N+" },
            { Key.Subtract, "N-" },
            { Key.Decimal, "N." },
            { Key.Divide, "N/" },
            { Key.LeftShift, "LSHIFT" },
            { Key.RightShift, "RSHIFT" },
            { Key.LeftCtrl, "LCTRL" },
            { Key.RightCtrl, "RCTRL" },
            { Key.LeftAlt, "LALT" },
            { Key.RightAlt, "RALT" },
            { Key.Return, "RTN" }
        };
    }
}
