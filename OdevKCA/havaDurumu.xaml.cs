using Newtonsoft.Json;

namespace OdevKCA;

public partial class havaDurumu : ContentPage
{
    private const string ApiKey = "ccc24efc4e4be8c3e36a6f2f6c8008e7"; // Senin API anahtarýn
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";
    public havaDurumu()
    {
        InitializeComponent();
    }

    private async void OnGetWeather_Clicked(object sender, EventArgs e)
    {
        try
        {
            var location = await GetLocationAsync();
            if (location != null)
            {
                var weather = await GetWeatherAsync(location.Latitude, location.Longitude);
                DisplayWeather(weather);
            }
            else
            {
                await DisplayAlert("Hata", "Konum bilgisi alýnamadý.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }

    }

    private async Task<Location> GetLocationAsync()
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location == null)
        {
            location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(30)
            });
        }
        return location;
    }

    private async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
    {
        using var httpClient = new HttpClient();
        var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&units=metric&appid={ApiKey}&lang=tr";
        var response = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<WeatherResponse>(response);
    }

    private void DisplayWeather(WeatherResponse weather)
    {
        WeatherLabel.Text = $"Þehir: {weather.Name}\n" +
                            $"Sýcaklýk: {weather.Main.Temp}°C\n" +
                            $"Durum: {weather.Weather[0].Description}";
    }

    public class WeatherResponse
    {
        public MainInfo Main { get; set; }
        public WeatherInfo[] Weather { get; set; }
        public string Name { get; set; }
    }

    public class MainInfo
    {
        public double Temp { get; set; }
    }

    public class WeatherInfo
    {
        public string Description { get; set; }
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