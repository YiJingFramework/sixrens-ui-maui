using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.UI.MAUI.Services.SixRens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        SixRensCore core;
        public MainPageViewModel(SixRensCore core)
        {
            this.core = core;
        }

        [ObservableProperty]
        int count;

        [RelayCommand]
        void IncreaseCount()
        {
            Count++;
        }
    }
}
