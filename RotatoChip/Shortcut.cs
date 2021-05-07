using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Crews.Utility.RotatoChip
{
    public class Shortcut
    {
        public string DeviceName { get; set; }
        public List<Key> Keys { get; set; }
        public Orientation Orientation { get; set; }

        public bool MatchesInput(List<Key> input) => Keys.Count == input.Count && SameKeys(Keys, input);

        private static bool SameKeys(List<Key> a, List<Key> b)
        {
            if (a.Count == 0 || b.Count == 0)
            {
                return false;
            }

            foreach (Key key in a)
            {
                if (!b.Contains<Key>(key))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
