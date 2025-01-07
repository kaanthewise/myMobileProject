using Newtonsoft.Json;

namespace OdevKCA;

public partial class doviz : ContentPage
{
    private const string ApiKey = "e81d906139e794bdf39b889e"; // API anahtarınız
    private const string ApiUrl = "https://api.exchangerate-api.com/v4/latest/";
    public doviz()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Kullanıcıdan alınan değerler
            if (string.IsNullOrWhiteSpace(AmountEntry.Text) || !decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                await DisplayAlert("Hata", "Geçerli bir miktar girin.", "Tamam");
                return;
            }

            if (SourceCurrencyPicker.SelectedItem == null || TargetCurrencyPicker.SelectedItem == null)
            {
                await DisplayAlert("Hata", "Lütfen para birimlerini seçin.", "Tamam");
                return;
            }

            string sourceCurrency = SourceCurrencyPicker.SelectedItem.ToString();
            string targetCurrency = TargetCurrencyPicker.SelectedItem.ToString();

            decimal rate = await GetExchangeRateAsync(sourceCurrency, targetCurrency);

            decimal result = amount * rate;

            ResultLabel.Text = $"{amount} {sourceCurrency} ≈ {result:F2} {targetCurrency}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata oluştu: {ex.Message}", "Tamam");
        }

    }

    private async Task<decimal> GetExchangeRateAsync(string sourceCurrency, string targetCurrency)
    {
        try
        {
            // API URL'sini oluştur
            string url = $"{ApiUrl}{sourceCurrency}";

            // API çağrısını yap
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);

            // API yanıtını kontrol et
            var exchangeData = JsonConvert.DeserializeObject<ExchangeRateResponse>(response);

            // Hedef para birimi var mı kontrol et
            if (exchangeData?.Rates != null && exchangeData.Rates.ContainsKey(targetCurrency))
            {
                return exchangeData.Rates[targetCurrency];
            }
            else
            {
                throw new Exception($"Hedef para birimi ({targetCurrency}) bulunamadı.");
            }
        }
        catch (Exception ex)
        {
            // Hata mesajını göster
            await DisplayAlert("Hata", $"Bir hata oluştu: {ex.Message}", "Tamam");
            return 0;
        }
    }

    public class ExchangeRateResponse
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
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