namespace OdevKCA
{
    public partial class register : ContentPage
    {
        public register()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Hata", "Tüm alanlarý doldurunuz.", "Tamam");
                return;
            }

            // Kullanýcý bilgilerini kaydet
            Preferences.Set("Username", username);
            Preferences.Set("Password", password);

            await DisplayAlert("Baþarýlý", "Kayýt tamamlandý. Þimdi giriþ yapabilirsiniz.", "Tamam");
            await Navigation.PopAsync(); // Giriþ sayfasýna geri dön

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }



}

