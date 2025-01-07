using Microsoft.Maui; // MAUI için gerekli ad alaný
#if ANDROID
using Android.Media; // Android için MediaPlayer
using Android.Content.Res; // OpenFd için gerekli
#endif

namespace OdevKCA
{
    public partial class alarmUyg : ContentPage
    {
        private bool sayiyorMu = false;
        private int kalanSure = 0;
        private int sabitSure = 0;
        private int sallamaSayisi = 0;
        private const int GerekliSallama = 3;
        private bool alarmCalisiyor = false;
#if ANDROID
        private MediaPlayer player; // Android için MediaPlayer tanýmý
#endif

        public alarmUyg()
        {
            InitializeComponent();
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private async void CounterBtn_Clicked(object sender, EventArgs e)
        {
            if (sayiyorMu) return;

            bool isValid = int.TryParse(sureEntry.Text, out sabitSure);
            if (!isValid || sabitSure <= 0)
            {
                sayac.Text = "Geçerli bir sayý giriniz";
                return;
            }

            if (kalanSure == 0)
            {
                kalanSure = sabitSure;
            }

            sayiyorMu = true;

            for (int i = kalanSure; i > 0; i--)
            {
                if (!sayiyorMu)
                {
                    kalanSure = i;
                    sayac.Text = $"{i} saniye kala durduruldu";
                    return;
                }

                sayac.Text = i.ToString();
                await Task.Delay(1000);
            }

            sayac.Text = "Süreniz bitti";
            sayiyorMu = false;
            kalanSure = 0;
            ÇalAlarm();
        }

        private void ÇalAlarm()
        {
#if ANDROID
            if (player == null)
            {
                player = new MediaPlayer();
                AssetFileDescriptor afd = Android.App.Application.Context.Assets.OpenFd("alarm.mp3");
                player.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                player.Prepare();
            }

            player.Start();
            alarmCalisiyor = true;
            sallamaSayisi = 0;
            Accelerometer.Start(SensorSpeed.UI);
#endif
        }

        private void sureDur_Clicked(object sender, EventArgs e)
        {
            sayiyorMu = false;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if (alarmCalisiyor && (Math.Abs(e.Reading.Acceleration.Z) > 1.8))
            {
                sallamaSayisi++;
                if (sallamaSayisi >= GerekliSallama)
                {
                    SusturAlarm();
                }
            }
        }

        private void SusturAlarm()
        {
#if ANDROID
            if (player != null && player.IsPlaying)
            {
                player.Stop();
                player.Release();
                player = null; // Kaynaklarý temizle
            }

            alarmCalisiyor = false;
            Accelerometer.Stop();
            sayac.Text = "Alarm susturuldu";
#endif
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
}
