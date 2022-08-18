using CommunityToolkit.Mvvm.ComponentModel;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class SettingsPageViewModel : ObservableObject, IQueryAttributable
    {
        private SixRensCore core;
        public SettingsPageViewModel(SixRensCore core)
        {
            this.core = core;
        }

        [ObservableProperty]
        string text;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("text", out var t) && t is string s)
            {
                Text = s;
            }
        }
    }
}
