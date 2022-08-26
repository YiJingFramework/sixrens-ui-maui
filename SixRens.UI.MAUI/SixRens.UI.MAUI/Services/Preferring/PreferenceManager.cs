using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Services.Preferring
{
    public sealed class PreferenceManager
    {
        private readonly IPreferences preferences;
        public PreferenceManager(IPreferences preferences)
        {
            this.preferences = preferences;
        }
        public string LastSelectedPreset
        {
            get => preferences.Get<string>(nameof(LastSelectedPreset), null);
            set => preferences.Set(nameof(LastSelectedPreset), value);
        }
        public void Clear()
        {
            this.preferences.Clear();
        }
    }
}
