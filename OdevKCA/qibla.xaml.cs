namespace OdevKCA;

public partial class qibla : ContentPage
{
	public qibla()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {System.Environment.Exit(0);

    }
}