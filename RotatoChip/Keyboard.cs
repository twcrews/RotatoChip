using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crews.Utility.RotatoChip
{
    class Keyboard
    {
        private List<Keys> Pressed { get; set; }
        public IReadOnlyCollection<Keys> PressedKeys => Pressed.AsReadOnly();

        public Keyboard() => Pressed = new List<Keys>();

        public void HandleKeyDown(object sender, HookEventArgs e) => Pressed.Add(e.Key);
        public void HandleKeyUp(object sender, HookEventArgs e) => Pressed.RemoveAll(key => key == e.Key);
    }
}
