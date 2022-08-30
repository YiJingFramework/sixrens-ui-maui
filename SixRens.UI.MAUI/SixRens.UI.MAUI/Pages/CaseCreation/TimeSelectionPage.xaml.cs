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
using System.Diagnostics;
using System.Linq;
using System.Text;
using YiJingFramework.StemsAndBranches;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class TimeSelectionPage : ContentPage
{
    private readonly Action<SelectedDateTime> applyBackAction;
    private readonly AppShell shell;
    public TimeSelectionPage(
        Action<SelectedDateTime> applyBackAction,
        AppShell shell)
    {
        this.applyBackAction = applyBackAction;
        this.shell = shell;

        InitializeComponent();

        var allStems = Enumerable.Range(1, 10)
            .Select(i => new HeavenlyStem(i).ToString("C"));
        var allBranches = Enumerable.Range(1, 12)
            .Select(i => new EarthlyBranch(i).ToString("C"));

        this.yearStemPicker.Items.AddOneByOne(allStems);
        this.monthStemPicker.Items.AddOneByOne(allStems);
        this.dateStemPicker.Items.AddOneByOne(allStems);
        this.timeStemPicker.Items.AddOneByOne(allStems);

        this.yearBranchPicker.Items.AddOneByOne(allBranches);
        this.monthBranchPicker.Items.AddOneByOne(allBranches);
        this.dateBranchPicker.Items.AddOneByOne(allBranches);
        this.timeBranchPicker.Items.AddOneByOne(allBranches);
    }

    internal void ApplySelectedDateTime(SelectedDateTime current)
    {
        {
            DateTime currentWestern;
            if (current.WesternDateTime.HasValue)
            {
                modePicker.SelectedIndex = 1;
                currentWestern = current.WesternDateTime.Value;
            }
            else
            {
                modePicker.SelectedIndex = 0;
                currentWestern = DateTime.Now;
            }
            var currentWesternDate = currentWestern.Date;
            this.datePicker.Date = currentWesternDate;
            this.timePicker.Time = currentWestern - currentWesternDate;
        }

        {
            var dateTime = current.DateTimeInformation;

            this.yearStemPicker.SelectedIndex = dateTime.年干.Index - 1;
            this.monthStemPicker.SelectedIndex = dateTime.月干.Index - 1;
            this.dateStemPicker.SelectedIndex = dateTime.日干.Index - 1;
            this.timeStemPicker.SelectedIndex = dateTime.时干.Index - 1;

            this.yearBranchPicker.SelectedIndex = dateTime.年支.Index - 1;
            this.monthBranchPicker.SelectedIndex = dateTime.月支.Index - 1;
            this.dateBranchPicker.SelectedIndex = dateTime.日支.Index - 1;
            this.timeBranchPicker.SelectedIndex = dateTime.时支.Index - 1;

            checkResultLabel.Text = string.Empty;
        }
    }
    private void OnModePickerIndexChanged(object sender, EventArgs e)
    {
        switch (modePicker.SelectedIndex)
        {
            case 0:
                layoutForMode1.IsVisible = false;
                layoutForMode0.IsVisible = true;
                break;
            case 1:
                layoutForMode0.IsVisible = false;
                layoutForMode1.IsVisible = true;
                break;
        }
    }

    private async void OnCommitWithStemsAndBranchesClicked(object sender, EventArgs e)
    {
        var yearStem = new HeavenlyStem(this.yearStemPicker.SelectedIndex + 1);
        var monthStem = new HeavenlyStem(this.monthStemPicker.SelectedIndex + 1);
        var dateStem = new HeavenlyStem(this.dateStemPicker.SelectedIndex + 1);
        var timeStem = new HeavenlyStem(this.timeStemPicker.SelectedIndex + 1);

        var yearBranch = new EarthlyBranch(this.yearBranchPicker.SelectedIndex + 1);
        var monthBranch = new EarthlyBranch(this.monthBranchPicker.SelectedIndex + 1);
        var dateBranch = new EarthlyBranch(this.dateBranchPicker.SelectedIndex + 1);
        var timeBranch = new EarthlyBranch(this.timeBranchPicker.SelectedIndex + 1);

        I年月日时信息 result;

        if (checkBeforeCommitCheckBox.IsChecked)
        {
            StringBuilder problems = new();

            var checkable = new 可检验年月日时(
                yearStem, yearBranch, monthBranch, dateStem, dateBranch, timeBranch,
                timeBranch.Index is >= 4 and < 10, new());
            Debug.Assert(checkable.检验昼夜());

            if (!checkable.检验年日阴阳())
                _ = problems.AppendLine("年或日的干支阴阳不匹配");
            else // 既然年日不正确，就不应该再判断月时了
            {
                if (checkable.月干 != monthStem)
                    _ = problems.AppendLine(
                        $"{yearStem:C}{yearBranch:C}年中只存在" +
                        $"{checkable.月干:C}{monthBranch:C}月");

                if (checkable.时干 != timeStem)
                    _ = problems.AppendLine(
                        $"{dateStem:C}{dateBranch:C}日中只存在" +
                        $"{checkable.时干:C}{timeBranch:C}时");
            }

            if (problems.Length is not 0)
            {
                this.checkResultLabel.Text = problems.ToString();
                return;
            }
            else
            {
                result = checkable;
            }
        }
        else
        {
            result = new 自定义年月日时(
                yearStem, yearBranch, monthStem, monthBranch,
                dateStem, dateBranch, timeStem, timeBranch,
                timeBranch.Index is >= 4 and < 10, new());
        }

        this.applyBackAction(new SelectedDateTime(result));
        _ = await shell.Navigation.PopAsync();
    }

    private async void OnCommitWithWesternClicked(object sender, EventArgs e)
    {
        var result = this.datePicker.Date + this.timePicker.Time;
        this.applyBackAction(new SelectedDateTime(result));
        _ = await shell.Navigation.PopAsync();
    }
}