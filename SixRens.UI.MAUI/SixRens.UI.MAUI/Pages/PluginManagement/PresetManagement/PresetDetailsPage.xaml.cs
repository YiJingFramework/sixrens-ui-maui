using CommunityToolkit.Maui.Views;
using Java.Lang;
using Microsoft.Maui.Storage;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Google.Crypto.Tink.Prf;
using static Android.Graphics.Path;

namespace SixRens.UI.MAUI.Pages.PluginManagement.PresetManagement
{
    public partial class PresetDetailsPage : ContentPage
    {
        private readonly SixRensCore core;
        private readonly 预设 preset;
        private readonly Action<预设> deleteAction;
        private readonly AppShell shell;
        public PresetDetailsPage(
            SixRensCore core,
            预设 preset,
            Action<预设> deleteAction,
            AppShell shell)
        {
            this.core = core;
            this.preset = preset;
            this.deleteAction = deleteAction;
            this.shell = shell;

            InitializeComponent();

            this.presetNameLabel.Text = preset.预设名;
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            deleteAction(preset);
            _ = await shell.Navigation.PopAsync();
        }
    }
}