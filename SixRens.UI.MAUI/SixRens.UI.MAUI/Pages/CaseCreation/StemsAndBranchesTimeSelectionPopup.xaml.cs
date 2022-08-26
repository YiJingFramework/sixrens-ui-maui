using CommunityToolkit.Maui.Views;
using SixRens.Core.������ʱ;
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

        this.yearStemPicker.SelectedIndex = dateTime.���.Index - 1;
        this.monthStemPicker.SelectedIndex = dateTime.�¸�.Index - 1;
        this.dateStemPicker.SelectedIndex = dateTime.�ո�.Index - 1;
        this.timeStemPicker.SelectedIndex = dateTime.ʱ��.Index - 1;

        this.yearBranchPicker.SelectedIndex = dateTime.��֧.Index - 1;
        this.monthBranchPicker.SelectedIndex = dateTime.��֧.Index - 1;
        this.dateBranchPicker.SelectedIndex = dateTime.��֧.Index - 1;
        this.timeBranchPicker.SelectedIndex = dateTime.ʱ֧.Index - 1;
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

        I������ʱ��Ϣ result;

        if (checkBeforeCommitCheckBox.IsChecked)
        {
            StringBuilder problems = new();

            var checkable = new �ɼ���������ʱ(
                yearStem, yearBranch, monthBranch, dateStem, dateBranch, timeBranch,
                timeBranch.Index is >= 4 and < 10, new());
            Debug.Assert(checkable.������ҹ());

            if (!checkable.������������())
                _ = problems.AppendLine("����յĸ�֧������ƥ��");

            if (checkable.�¸� != monthStem)
                _ = problems.AppendLine(
                    $"{yearStem:C}{yearBranch:C}����ֻ����" +
                    $"{checkable.�¸�:C}{monthBranch:C}��");

            if (checkable.ʱ�� != timeStem)
                _ = problems.AppendLine(
                    $"{dateStem:C}{dateBranch:C}����ֻ����" +
                    $"{checkable.ʱ��:C}{timeBranch:C}ʱ");

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
            result = new �Զ���������ʱ(
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