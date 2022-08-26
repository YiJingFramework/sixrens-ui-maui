using CommunityToolkit.Maui.Views;
using SixRens.Core.年月日时;
using System;
using System.Diagnostics;
using System.Text;
using YiJingFramework.StemsAndBranches;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class StemsAndBranchesTimeSelectionPopup : Popup
{
    public StemsAndBranchesTimeSelectionPopup(SelectedDateTime currentValue)
    {
        InitializeComponent();

        var allStems = Enumerable.Range(1, 10)
            .Select(i => new HeavenlyStem(i).ToString("C")).ToArray();
        var allBranches = Enumerable.Range(1, 12)
            .Select(i => new EarthlyBranch(i).ToString("C")).ToArray();

        this.yearStemPicker.ItemsSource = allStems;
        this.monthStemPicker.ItemsSource = allStems;
        this.dateStemPicker.ItemsSource = allStems;
        this.timeStemPicker.ItemsSource = allStems;

        this.yearBranchPicker.ItemsSource = allBranches;
        this.monthBranchPicker.ItemsSource = allBranches;
        this.dateBranchPicker.ItemsSource = allBranches;
        this.timeBranchPicker.ItemsSource = allBranches;

        var dateTime = currentValue.DateTimeInformation;

        this.yearStemPicker.SelectedIndex = dateTime.年干.Index - 1;
        this.monthStemPicker.SelectedIndex = dateTime.月干.Index - 1;
        this.dateStemPicker.SelectedIndex = dateTime.日干.Index - 1;
        this.timeStemPicker.SelectedIndex = dateTime.时干.Index - 1;

        this.yearBranchPicker.SelectedIndex = dateTime.年支.Index - 1;
        this.monthBranchPicker.SelectedIndex = dateTime.月支.Index - 1;
        this.dateBranchPicker.SelectedIndex = dateTime.日支.Index - 1;
        this.timeBranchPicker.SelectedIndex = dateTime.时支.Index - 1;
    }

    private void Commit(object sender, EventArgs e)
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

            if (checkable.月干 != monthStem)
                _ = problems.AppendLine(
                    $"{yearStem:C}{yearBranch:C}年中只存在" +
                    $"{checkable.月干:C}{monthBranch:C}月");

            if (checkable.时干 != timeStem)
                _ = problems.AppendLine(
                    $"{dateStem:C}{dateBranch:C}日中只存在" +
                    $"{checkable.时干:C}{timeBranch:C}时");

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

        this.Close(new SelectedDateTime(result));
    }

    private void Cancel(object sender, EventArgs e)
    {
        this.Close(null);
    }
}