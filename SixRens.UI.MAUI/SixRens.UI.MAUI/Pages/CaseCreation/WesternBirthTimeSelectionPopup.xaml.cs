using CommunityToolkit.Maui.Views;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class WesternBirthTimeSelectionPopup : Popup
{
	public WesternBirthTimeSelectionPopup(DateTime? currentValue)
	{
		InitializeComponent();

        var dateTime = currentValue ?? DateTime.Now;
        this.datePicker.Date = dateTime;
    }

	private void Commit(object sender, EventArgs e)
    {
        this.Close(this.datePicker.Date);
    }
    private void Cancel(object sender, EventArgs e)
    {
        this.Close(null);
    }
}