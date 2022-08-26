using CommunityToolkit.Maui.Views;
using Kotlin.Time;
using SixRens.Core.壬式生成;
using SixRens.Core.年月日时;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.Preferring;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.Tools.Extensions;
using SixRens.UI.MAUI.Tools.Querying;
using System.ComponentModel;
using System.Linq;
using System.Text;
using YiJingFramework.StemsAndBranches;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class CaseCreationPage : ContentPage
{
    readonly AppShell shell;
    readonly SixRensCore core;
    readonly PreferenceManager preferenceManager;

    readonly BindingList<string> dayNightPickerItems;
    readonly BindingList<string> theSunPickerItems;
    public CaseCreationPage(SixRensCore core, AppShell shell, PreferenceManager preferenceManager)
    {
        this.core = core;
        this.preferenceManager = preferenceManager;

        this.shell = shell;
        
        InitializeComponent();

        dayNightPickerItems = new() {
            "自动（无）",
            "昼占",
            "夜占"
        };
        this.dayNightPicker.ItemsSource = dayNightPickerItems;
        this.dayNightPicker.SelectedIndex = 0;

        theSunPickerItems = new();
        theSunPickerItems.AddOneByOne(
            Enumerable.Range(1, 12)
            .Select(i => new EarthlyBranch(i).ToString("C"))
            .Prepend("自动（无）"));
        this.theSunPicker.ItemsSource = theSunPickerItems;
        this.theSunPicker.SelectedIndex = 0;

        SetDateTime(new(DateTime.Now));
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        RefreshPresets();
    }

    private void SetDateTime(SelectedDateTime dateTime)
    {
        this.selectedDateTimeLabel.BindingContext = new ShowingDateTime(dateTime);
        dayNightPickerItems[0] = dateTime.DateTimeInformation.昼占 ? "自动（昼占）" : "自动（夜占）";
        if (dateTime.ProvidesTheSun)
            theSunPickerItems[0] = dateTime.DateTimeInformation.月将.ToString("C");
        else
            theSunPickerItems[0] = "自动（无）";
    }

    private void RefreshPresets()
    {
        var presets = core.PresetManager.预设列表.ToArray();
        this.presetPicker.ItemsSource = presets;

        var lastUsed = preferenceManager.LastSelectedPreset;
        var selected = presets.FirstOrDefault(p => p.预设名 == lastUsed, null);
        if (selected is null && presets.Length is not 0)
            selected = presets[0];
        
        this.presetPicker.SelectedItem = selected;
    }

    private async void CreateCase(object sender, EventArgs e)
    {
        var preset = this.presetPicker.SelectedItem as 预设;
        var parsed = preset is null ? null : core.PluginPackageManager.解析预设(preset);
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
                var dateTime = (ShowingDateTime)selectedDateTimeLabel.BindingContext;
                var plate = new 壬式(
                    new 起课参数(dateTime.DateTime.DateTimeInformation, null, Array.Empty<年命>()), parsed);
                var dCase = plate.创建占例();
                dCase.西历时间 = dateTime.DateTime.WesternDateTime;
                var query = SingleParameterQuery
                    .Create(new Main.MainPageQueryParameters(null, dCase));
                await shell.GoToAsync("//main", query);
            }
        }
    }

    private async void SelectDateTime(object sender, EventArgs e)
    {
        var current = (ShowingDateTime)selectedDateTimeLabel.BindingContext;

        const string western = "通过西历";
        const string stemsAndBranches = "通过干支";
        var mode =
            await this.DisplayActionSheet("选择模式：", "取消", null, western, stemsAndBranches);
        switch (mode)
        {
            case western:
            {
                var result = (SelectedDateTime)
                    await this.ShowPopupAsync(new WesternTimeSelectionPopup(current.DateTime));
                if (result is not null)
                    this.SetDateTime(result);
                break;
            }
            case stemsAndBranches:
            {
                var result = (SelectedDateTime)
                    await this.ShowPopupAsync(new StemsAndBranchesTimeSelectionPopup(current.DateTime));
                if (result is not null)
                    this.SetDateTime(result);
                break;
            }
        }
    }

    private void PresetSelected(object sender, EventArgs e)
    {
        if(presetPicker.SelectedItem is not null)
        {
            // 给 ItemsSource 赋值时会导致事件被触发。
            preferenceManager.LastSelectedPreset = ((预设)presetPicker.SelectedItem).预设名;
        }
    }

}