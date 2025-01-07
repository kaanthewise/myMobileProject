namespace OdevKCA;

public partial class kalori : ContentPage
{
    public kalori(int steps, double calories)
    {
        InitializeComponent();

        if (StepsLabel != null && CaloriesLabel != null)
        {
            StepsLabel.Text = $"Ad�m Say�s�: {steps}";
            CaloriesLabel.Text = $"Yak�lan Kalori: {calories:F2} kcal";
        }
        else
        {
            DisplayAlert("Hata", "Veriler do�ru �ekilde y�klenmedi.", "Tamam");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }
}