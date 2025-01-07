using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Plugin.Maui.Audio;

namespace OdevKCA;

public partial class alarm : ContentPage
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer _player;
    private bool sayiyorMu = false;
    private int kalanSure = 0;
    private int sabitSure = 0;

    private int shakeCount = 0; // Sallama sayýsý
    private const double ShakeThreshold = 1.5; // Eþik deðer
    private DateTime lastShakeTime;
    public alarm(IAudioManager audioManager)
	{
		InitializeComponent();
        _audioManager = audioManager;

        Accelerometer.ReadingChanged += OnAccelerometerReadingChanged;
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
        var stream = await FileSystem.OpenAppPackageFileAsync("alarm.mp3");
        _player = _audioManager.CreatePlayer(stream);

        _player.Play();

        Accelerometer.Start(SensorSpeed.Game);
        sayac.Text = "Süreniz bitti";
        sayiyorMu = false;
        kalanSure = 0;

    }

    private void sureDur_Clicked(object sender, EventArgs e)
    {
        sayiyorMu = false;
    }

    private void OnAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e) 
    {
        var x = e.Reading.Acceleration.X;
        var y = e.Reading.Acceleration.Y;
        var z = e.Reading.Acceleration.Z;

        // Ivme deðiþimini kontrol et
        var totalAcceleration = Math.Sqrt(x * x + y * y + z * z);

        if (totalAcceleration > ShakeThreshold)
        {
            var now = DateTime.UtcNow;

            if ((now - lastShakeTime).TotalMilliseconds > 500)
            {
                lastShakeTime = now;
                shakeCount++;
                if (shakeCount >= 3)
                {
                    _player.Stop(); // Alarmý durdur
                    shakeCount = 0; // Sayacý sýfýrla
                    Accelerometer.Stop(); // Ivmeölçeri durdur
                    sayac.Text = "Alarm durduruldu!";
                }
            }

        }
    }
}