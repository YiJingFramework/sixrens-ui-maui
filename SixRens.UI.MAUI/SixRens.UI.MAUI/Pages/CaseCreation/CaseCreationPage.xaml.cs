using CommunityToolkit.Maui.Views;
using SixRens.Core.��ʽ����;
using SixRens.Core.������ʱ;
using SixRens.Core.�������.Ԥ�����;
using SixRens.UI.MAUI.Services.Preferring;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.Tools.Querying;
using System.Text;

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

        SetDateTime(new(DateTime.Now));
        RefreshPresets();

        this.NavigatedFrom += (_, _) => RefreshPresets();
    }

    private void SetDateTime(SelectedDateTime dateTime)
    {
        this.selectedDateTimeLabel.BindingContext = new ShowingDateTime(dateTime);
    }
    private void RefreshPresets()
    {
        this.presetCollectionView.ItemsSource = core.PresetManager.Ԥ���б�.ToArray();
        var lastUsed = preferenceManager.LastUsedPreset;
        this.presetCollectionView.SelectedItem = 
            core.PresetManager.Ԥ���б�.FirstOrDefault(p => p.Ԥ���� == lastUsed, null);
    }

    private async void CreateCase(object sender, EventArgs e)
    {
        var preset = this.presetCollectionView.SelectedItem as Ԥ��;
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
            preferenceManager.LastUsedPreset = preset.Ԥ����;

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
                this.SetDateTime(result);
                break;
            }
            case stemsAndBranches:
            {
                var result = (SelectedDateTime)
                    await this.ShowPopupAsync(new StemsAndBranchesTimeSelectionPopup(current.DateTime));
                this.SetDateTime(result);
                break;
            }
        }
    }
}