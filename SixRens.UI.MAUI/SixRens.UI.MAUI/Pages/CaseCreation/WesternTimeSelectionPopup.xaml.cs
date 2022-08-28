using CommunityToolkit.Maui.Views;

namespace SixRens.UI.MAUI.Pages.CaseCreation;

public partial class WesternTimeSelectionPopup : Popup
{
	public WesternTimeSelectionPopup(SelectedDateTime currentValue)
	{
		InitializeComponent();

        var dateTime = currentValue.WesternDateTime ?? DateTime.Now;
        this.datePicker.Date = dateTime;
        this.timePicker.Time = dateTime - dateTime.Date;
    }

	private void Commit(object sender, EventArgs e)
    {
        var dateTime = this.datePicker.Date + this.timePicker.Time;
        this.Close(new SelectedDateTime(dateTime));
    }
    private void Cancel(object sender, EventArgs e)
    {
        this.Close(null);
    }
}