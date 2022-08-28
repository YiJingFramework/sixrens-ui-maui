﻿using CommunityToolkit.Maui.Views;
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

    public CaseCreationPage(SixRensCore core, AppShell shell, PreferenceManager preferenceManager)
    {
        this.core = core;
        this.preferenceManager = preferenceManager;

        this.shell = shell;
        
        InitializeComponent();

        this.dayNightPicker.Items.AddOneByOne(new[] {
            "自动（无）",
            "昼占",
            "夜占"
        });
        this.dayNightPicker.SelectedIndex = 0;

        theSunPicker.Items.AddOneByOne(
            Enumerable.Range(1, 12)
            .Select(i => new EarthlyBranch(i).ToString("C"))
            .Prepend("自动（无）"));
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
        dayNightPicker.Items[0] = dateTime.DateTimeInformation.昼占 ? "自动（昼占）" : "自动（夜占）";
        if(dayNightPicker.SelectedIndex is 0)
        {
            dayNightPicker.SelectedIndex = -1;
            dayNightPicker.SelectedIndex = 0;
        }

        if (dateTime.ProvidesTheSun)
            theSunPicker.Items[0] = $"自动（{dateTime.DateTimeInformation.月将:C}）";
        else
            theSunPicker.Items[0] = "自动（无）";
        if (theSunPicker.SelectedIndex is 0)
        {
            theSunPicker.SelectedIndex = -1;
            theSunPicker.SelectedIndex = 0;
        }
    }

    // 给 ItemsSource 赋值时会导致选择 null ，故设一变量进行判断。
    private bool isRefreshingPresets;
    private void RefreshPresets()
    {
        isRefreshingPresets = true;

        var presets = core.PresetManager.预设列表.ToArray();
        this.presetPicker.ItemsSource = presets;

        var lastUsed = preferenceManager.LastSelectedPreset;
        var selected = presets.FirstOrDefault(p => p.预设名 == lastUsed, null);
        if (selected is not null)
        {
            this.presetPicker.SelectedItem = selected;
            isRefreshingPresets = false;
        }
        else
        {
            isRefreshingPresets = false;
            if (presets.Length > 0)
                this.presetPicker.SelectedItem = presets[0];
        }
    }

    private async void CreateCase(object sender, EventArgs e)
    {
        var dateTime = (ShowingDateTime)selectedDateTimeLabel.BindingContext;
        var modifiedDateTime = dateTime.DateTime.DateTimeInformation;
        if (theSunPicker.SelectedIndex is 0)
        {
            if(!dateTime.DateTime.ProvidesTheSun)
            {
                await this.DisplayAlert(
                    "没有选择月将",
                    "无法自动确定月将，需要手动选择。",
                    "确定");
                return;
            }
        }
        else
        {
            modifiedDateTime = modifiedDateTime.修改信息(
                月将: new(theSunPicker.SelectedIndex));
        }

        if (dayNightPicker.SelectedIndex is not 0)
            modifiedDateTime = modifiedDateTime.修改信息(
                昼占: dayNightPicker.SelectedIndex is 1);

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
                var plate = new 壬式(
                    new 起课参数(modifiedDateTime,
                    null, Array.Empty<年命>()),
                    parsed);
                var dCase = plate.创建占例();
                dCase.西历时间 = dateTime.DateTime.WesternDateTime;
#warning 下一行只是为测试用
                dCase.断语 = dCase.可读文本化();
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
        if(!isRefreshingPresets)
            preferenceManager.LastSelectedPreset = ((预设)presetPicker.SelectedItem).预设名;
    }

    private async void AddGenderAndBirth(object sender, EventArgs e)
    {
        var result = await this.ShowPopupAsync(new GenderAndBirthSelectionPopup());
    }
}