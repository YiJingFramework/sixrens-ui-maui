using SixRens.Core.壬式生成;
using SixRens.Core.年月日时;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.Tools.Querying;
using System.Text;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class CaseCreationPage : ContentPage
{
    readonly AppShell shell;
    readonly SixRensCore core;
    public CaseCreationPage(SixRensCore core,AppShell shell)
    {
        this.core = core;

        this.shell = shell;
        InitializeComponent();
        
        RefreshDateTime();
        RefreshPresets();
        
        this.NavigatedFrom += this.OnNavigatedFrom;
    }

    private void OnNavigatedFrom(object sender, NavigatedFromEventArgs e)
    {
        RefreshPresets();
    }

    private void RefreshDateTime()
    {
        var now = DateTime.Now;
        this.datePicker.Date = now;
        this.timePicker.Time = now - now.Date;
    }
    private void RefreshPresets()
    {
        this.presetCollectionView.ItemsSource = core.PresetManager.预设列表;
    }

    private async void CreateCase(object sender, EventArgs e)
    {
        var parsed = this.presetCollectionView.SelectedItem is 预设 preset ?
             core.PluginPackageManager.解析预设(preset) : null;
        if (parsed is null)
        {
            await this.DisplayAlert(
                "不可用的预设", 
                "此预设不可用，请检查预设是否选择完整，并确保安装了对应插件", 
                "确定");
        }
        else
        {
            StringBuilder stringBuilder = new();
            _ = stringBuilder.AppendLine("发现了以下问题，但仍然可以起课，要继续么？");
            var initial = stringBuilder.Length;

            if (parsed.存在未显式指定的课体)
                _ = stringBuilder.AppendLine("存在未显式指定的课体（视为启用）");
            if (parsed.存在指定启用但未找到的课体)
                _ = stringBuilder.AppendLine("存在指定启用但未找到的课体（视为禁用）");
            if (parsed.存在同时指定了启用和禁用的神煞)
                _ = stringBuilder.AppendLine("存在同时指定了启用和禁用的神煞（视为禁用）");
            if (parsed.存在未显式指定的神煞)
                _ = stringBuilder.AppendLine("存在未显式指定的神煞（视为启用）");
            if (parsed.存在指定启用但未找到的神煞)
                _ = stringBuilder.AppendLine("存在指定启用但未找到的神煞（视为禁用）");
            if (parsed.存在同时指定了启用和禁用的课体)
                _ = stringBuilder.AppendLine("存在同时指定了启用和禁用的课体（视为禁用）");

            if (stringBuilder.Length == initial || 
                await this.DisplayAlert(
                    "发现问题",
                    stringBuilder.ToString(),
                    "继续", "取消"))
            {
                var dateTime = this.datePicker.Date + this.timePicker.Time;
                var plate = new 壬式(
                    new 起课参数(new 真实年月日时(dateTime), null, Array.Empty<年命>()), parsed);
                var dCase = plate.创建占例();
                dCase.西历时间 = dateTime;
                var query = SingleParameterQuery
                    .Create(new Main.MainPageQueryParameters(null, dCase));
                await shell.GoToAsync("//main", query);
            }
        }
    }
}