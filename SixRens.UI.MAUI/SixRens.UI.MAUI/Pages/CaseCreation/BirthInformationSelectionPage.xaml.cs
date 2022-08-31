using CommunityToolkit.Maui.Views;
using Kotlin.Time;
using SixRens.Api.实体;
using SixRens.Core.壬式生成;
using SixRens.Core.年月日时;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.Preferring;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.Tools.Extensions;
using SixRens.UI.MAUI.Tools.Querying;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YiJingFramework.StemsAndBranches;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class BirthInformationSelectionPage : ContentPage
{
    private readonly Action<SelectedBirthInformationList> applyBackAction;
    private readonly AppShell shell;

    private readonly BirthInformationGroup ownerBinding;
    private readonly BirthInformationGroup notOwnerBinding;
    public BirthInformationSelectionPage(
        SelectedBirthInformationList current,
        Action<SelectedBirthInformationList> applyBackAction,
        AppShell shell)
    {
        this.applyBackAction = applyBackAction;
        this.shell = shell;

        InitializeComponent();

        {
            var allBranches = Enumerable.Range(1, 12)
                .Select(i => new EarthlyBranch(i).ToString("C"));

            this.birthPicker.Items.AddOneByOne(allBranches);
            this.agePicker.Items.AddOneByOne(allBranches.Prepend("自动"));

            this.genderPicker.SelectedIndex = 1;
            this.birthPicker.SelectedIndex = 0;
            this.agePicker.SelectedIndex = 0;
        }

        {
            ownerBinding = new("课主");
            notOwnerBinding = new("非课主");

            if (current.PlateOwners is not null)
            {
                ownerBinding.Add(current.PlateOwners);
                this.asOwnerPicker.SelectedIndex = 1;
            }
            else
            {
                this.asOwnerPicker.SelectedIndex = 0;
            }
            notOwnerBinding.AddOneByOne(current.Others);

            this.collectionView.ItemsSource = new BirthInformationGroup[] {
                ownerBinding,
                notOwnerBinding
            };
        }
    }

    private void AddItemClicked(object sender, EventArgs e)
    {
        var ageIndex = this.agePicker.SelectedIndex;
        var information = new BirthInformation(
            this.genderPicker.SelectedIndex is 0 ? 性别.女 : 性别.男,
            new(this.birthPicker.SelectedIndex + 1),
            ageIndex is 0 ? null : new(ageIndex));
        if(this.asOwnerPicker.SelectedIndex is 0)
        {
            if(ownerBinding.Count is 0)
            {
                ownerBinding.Add(information);
                this.asOwnerPicker.SelectedIndex = 1;
            }
            else
            {
                var value = this.ownerBinding[0];
                this.ownerBinding.RemoveAt(0);
                this.notOwnerBinding.Add(value);
                this.ownerBinding.Add(information);
            }
        }
        else
            this.notOwnerBinding.Add(information);
    }

    private async void OnCommitClicked(object sender, EventArgs e)
    {
        this.applyBackAction(
            new(this.ownerBinding.FirstOrDefault(defaultValue: null), this.notOwnerBinding));
        _ = await shell.Navigation.PopAsync();
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var birthInformation = (BirthInformation)button.BindingContext;
        if(this.ownerBinding.Remove(birthInformation))
        {
            this.asOwnerPicker.SelectedIndex = 0;
        }
        else
        {
            var result = this.notOwnerBinding.Remove(birthInformation);
            Debug.Assert(result is true);
        }
    }
}