namespace OdevKCA;

public partial class kalori : ContentPage
{
    public kalori(int steps, double calories)
    {
        InitializeComponent();

        if (StepsLabel != null && CaloriesLabel != null)
        {
            StepsLabel.Text = $"Adým Sayýsý: {steps}";
            CaloriesLabel.Text = $"Yakýlan Kalori: {calories:F2} kcal";
        }
        else
        {
            DisplayAlert("Hata", "Veriler doðru þekilde yüklenmedi.", "Tamam");
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