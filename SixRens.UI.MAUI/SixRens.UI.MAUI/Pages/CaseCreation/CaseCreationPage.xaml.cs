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

        RefreshDateTime();
        RefreshPresets();

        this.NavigatedFrom += (_, _) => RefreshPresets();
    }

    private void RefreshDateTime()
    {
        var now = DateTime.Now;
        this.datePicker.Date = now;
        this.timePicker.Time = now - now.Date;
    }
    private void RefreshPresets()
    {
        this.presetCollectionView.ItemsSource = core.PresetManager.Ԥ���б�;
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
                var dateTime = this.datePicker.Date + this.timePicker.Time;
                var plate = new ��ʽ(
                    new ��β���(new ��ʵ������ʱ(dateTime), null, Array.Empty<����>()), parsed);
                var dCase = plate.����ռ��();
                dCase.����ʱ�� = dateTime;
                var query = SingleParameterQuery
                    .Create(new Main.MainPageQueryParameters(null, dCase));
                await shell.GoToAsync("//main", query);
            }
        }
    }
}