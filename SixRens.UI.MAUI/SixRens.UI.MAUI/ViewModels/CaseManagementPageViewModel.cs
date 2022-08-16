using CommunityToolkit.Mvvm.ComponentModel;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class CaseManagementPageViewModel : ObservableObject
    {
        private SixRensCore core;
        public CaseManagementPageViewModel(SixRensCore core)
        {
            this.core = core;
        }
    }
}
