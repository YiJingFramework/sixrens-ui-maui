using CommunityToolkit.Mvvm.ComponentModel;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class CaseCreationPageViewModel : ObservableObject
    {
        private SixRensCore core;
        public CaseCreationPageViewModel(SixRensCore core)
        {
            this.core = core;
        }
    }
}
