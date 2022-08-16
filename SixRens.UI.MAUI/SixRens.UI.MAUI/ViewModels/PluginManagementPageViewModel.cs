using CommunityToolkit.Mvvm.ComponentModel;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class PluginManagementPageViewModel : ObservableObject
    {
        private SixRensCore core;
        public PluginManagementPageViewModel(SixRensCore core)
        {
            this.core = core;
        }
    }
}
