using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Crews.Utility.RotatoChip
{
    class Keyboard
    {
        private List<Key> Pressed { get; set; }
        public IReadOnlyCollection<Key> PressedKeys => Pressed.AsReadOnly();

        public event EventHandler PressedKeysChanged;

        public Keyboard() => Pressed = new List<Key>();

        public void HandleKeyDown(object sender, HookEventArgs e)
        {
            Pressed.Add(e.Key);
            PressedKeysChanged.Invoke(this, e);
        }

        public void HandleKeyUp(object sender, HookEventArgs e)
        {
            Pressed.RemoveAll(key => key == e.Key);
            PressedKeysChanged.Invoke(this, e);
        }

        public static Key GetKeyFromLegacy(LegacyKeys key) => (Key)Enum.Parse(typeof(Key), key.ToString());
    }
}
