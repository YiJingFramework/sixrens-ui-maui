using CommunityToolkit.Maui.Views;
using SixRens.UI.MAUI.Tools.Extensions;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class GenderAndBirthSelectionPopup : Popup
{
	public GenderAndBirthSelectionPopup()
	{
		InitializeComponent();
    }

	private void Commit(object sender, EventArgs e)
    {
        this.Close();
    }
    private void Cancel(object sender, EventArgs e)
    {
        this.Close(null);
    }

    private async void SelectBirthWithWestern(object sender, EventArgs e)
    {
        var result = await this.ShowPopupAsync(new WesternBirthTimeSelectionPopup(null));
    }
}