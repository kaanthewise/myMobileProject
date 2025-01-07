namespace OdevKCA
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();
        }


        private async void register_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new register());

        }

        private async void login_Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            // Kayıtlı kullanıcı bilgilerini kontrol et
            string savedUsername = Preferences.Get("Username", null);
            string savedPassword = Preferences.Get("Password", null);

            if (username == savedUsername && password == savedPassword)
            {
                await DisplayAlert("Başarılı", "Giriş yapıldı!", "Tamam");
                Application.Current.MainPage = new AppShell(); // Ana sayfaya yönlendirme
            }
            else
            {
                await DisplayAlert("Hata", "Kullanıcı adı veya şifre hatalı.", "Tamam");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);

        }
    }

}
