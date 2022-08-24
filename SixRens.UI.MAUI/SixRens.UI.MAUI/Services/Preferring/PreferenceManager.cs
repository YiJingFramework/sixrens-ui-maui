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
        public string LastUsedPreset
        {
            get => preferences.Get<string>(nameof(LastUsedPreset), null);
            set => preferences.Set(nameof(LastUsedPreset), value);
        }
        public void Clear()
        {
            this.preferences.Clear();
        }
    }
}
