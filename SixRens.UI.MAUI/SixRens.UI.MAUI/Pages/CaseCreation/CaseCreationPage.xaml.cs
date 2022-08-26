using CommunityToolkit.Maui.Views;
using Kotlin.Time;
using SixRens.Core.��ʽ����;
using SixRens.Core.������ʱ;
using SixRens.Core.�������.Ԥ�����;
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
            "�Զ����ޣ�",
            "��ռ",
            "ҹռ"
        };
        this.dayNightPicker.ItemsSource = dayNightPickerItems;
        this.dayNightPicker.SelectedIndex = 0;

        theSunPickerItems = new();
        theSunPickerItems.AddOneByOne(
            Enumerable.Range(1, 12)
            .Select(i => new EarthlyBranch(i).ToString("C"))
            .Prepend("�Զ����ޣ�"));
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
        dayNightPickerItems[0] = dateTime.DateTimeInformation.��ռ ? "�Զ�����ռ��" : "�Զ���ҹռ��";
        if (dateTime.ProvidesTheSun)
            theSunPickerItems[0] = dateTime.DateTimeInformation.�½�.ToString("C");
        else
            theSunPickerItems[0] = "�Զ����ޣ�";
    }

    private void RefreshPresets()
    {
        var presets = core.PresetManager.Ԥ���б�.ToArray();
        this.presetPicker.ItemsSource = presets;

        var lastUsed = preferenceManager.LastSelectedPreset;
        var selected = presets.FirstOrDefault(p => p.Ԥ���� == lastUsed, null);
        if (selected is null && presets.Length is not 0)
            selected = presets[0];
        
        this.presetPicker.SelectedItem = selected;
    }

    private async void CreateCase(object sender, EventArgs e)
    {
        var preset = this.presetPicker.SelectedItem as Ԥ��;
        var parsed = preset is null ? null : core.PluginPackageManager.����Ԥ��(preset);
        if (parsed is null)
        {
            await this.DisplayAlert(
                "�����õ�Ԥ��", 
                "��Ԥ�費���ã�����Ԥ���Ƿ�ѡ����������ȷ����װ�˶�Ӧ���", 
                "ȷ��");
        }
        else
        {
            StringBuilder stringBuilder = new();
            _ = stringBuilder.AppendLine("�������������⣬����Ȼ������Σ�Ҫ����ô��");
            var initial = stringBuilder.Length;

            if (parsed.����δ��ʽָ���Ŀ���)
                _ = stringBuilder.AppendLine("����δ��ʽָ���Ŀ��壨��Ϊ���ã�");
            if (parsed.����ָ�����õ�δ�ҵ��Ŀ���)
                _ = stringBuilder.AppendLine("����ָ�����õ�δ�ҵ��Ŀ��壨��Ϊ���ã�");
            if (parsed.����ͬʱָ�������úͽ��õ���ɷ)
                _ = stringBuilder.AppendLine("����ͬʱָ�������úͽ��õ���ɷ����Ϊ���ã�");
            if (parsed.����δ��ʽָ������ɷ)
                _ = stringBuilder.AppendLine("����δ��ʽָ������ɷ����Ϊ���ã�");
            if (parsed.����ָ�����õ�δ�ҵ�����ɷ)
                _ = stringBuilder.AppendLine("����ָ�����õ�δ�ҵ�����ɷ����Ϊ���ã�");
            if (parsed.����ͬʱָ�������úͽ��õĿ���)
                _ = stringBuilder.AppendLine("����ͬʱָ�������úͽ��õĿ��壨��Ϊ���ã�");

            if (stringBuilder.Length == initial || 
                await this.DisplayAlert(
                    "��������",
                    stringBuilder.ToString(),
                    "����", "ȡ��"))
            {
                var dateTime = (ShowingDateTime)selectedDateTimeLabel.BindingContext;
                var plate = new ��ʽ(
                    new ��β���(dateTime.DateTime.DateTimeInformation, null, Array.Empty<����>()), parsed);
                var dCase = plate.����ռ��();
                dCase.����ʱ�� = dateTime.DateTime.WesternDateTime;
                var query = SingleParameterQuery
                    .Create(new Main.MainPageQueryParameters(null, dCase));
                await shell.GoToAsync("//main", query);
            }
        }
    }

    private async void SelectDateTime(object sender, EventArgs e)
    {
        var current = (ShowingDateTime)selectedDateTimeLabel.BindingContext;

        const string western = "ͨ������";
        const string stemsAndBranches = "ͨ����֧";
        var mode =
            await this.DisplayActionSheet("ѡ��ģʽ��", "ȡ��", null, western, stemsAndBranches);
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
            // �� ItemsSource ��ֵʱ�ᵼ���¼���������
            preferenceManager.LastSelectedPreset = ((Ԥ��)presetPicker.SelectedItem).Ԥ����;
        }
    }

}