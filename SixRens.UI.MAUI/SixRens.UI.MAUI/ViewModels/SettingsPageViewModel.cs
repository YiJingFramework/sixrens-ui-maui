using CommunityToolkit.Mvvm.ComponentModel;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class SettingsPageViewModel : ObservableObject
    {
        private SixRensCore core;
        public SettingsPageViewModel(SixRensCore core)
        {
            this.core = core;
        }
    }
}
