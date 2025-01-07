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
                await DisplayAlert("Hata", "T�m alanlar� doldurunuz.", "Tamam");
                return;
            }

            // Kullan�c� bilgilerini kaydet
            Preferences.Set("Username", username);
            Preferences.Set("Password", password);

            await DisplayAlert("Ba�ar�l�", "Kay�t tamamland�. �imdi giri� yapabilirsiniz.", "Tamam");
            await Navigation.PopAsync(); // Giri� sayfas�na geri d�n

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }



}

