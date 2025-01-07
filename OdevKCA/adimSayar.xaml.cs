using Plugin.Maui.Pedometer;

namespace OdevKCA;

public partial class adimSayar : ContentPage
{
    public adimSayar()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Pedometer.Default.IsSupported)
        {
            Pedometer.Default.ReadingChanged += OnReadingChanged;
            Pedometer.Default.Start();
        }
        else
        {
            DisplayAlert("Hata", "Adım sayar bu cihazda desteklenmiyor.", "Tamam");
        }
    }

    private void OnReadingChanged(object sender, PedometerData e)
    {
        Dispatcher.Dispatch(() =>
        {
            StepsLabel.Text = $" 👟 Adım Sayısı: {e.NumberOfSteps}";
        });
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Pedometer.Default.ReadingChanged -= OnReadingChanged;
        Pedometer.Default.Stop();

    }

    private async void hesapla_Clicked(object sender, EventArgs e)
    {
        int steps = 0;
        try
        {
            // Adım sayısını doğru bir şekilde almak için string işlemi
            steps = Convert.ToInt32(StepsLabel.Text.Split(':')[1].Trim());
        }
        catch (Exception ex)
        {
            DisplayAlert("Hata", $"Adım sayısı alınırken hata oluştu: {ex.Message}", "Tamam");
            return;
        }

        double calories = steps * 0.04;
        await Navigation.PushAsync(new kalori(steps, calories));
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }
}