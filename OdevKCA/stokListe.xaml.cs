using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using Microsoft.Maui.Storage;

namespace OdevKCA
{
    public partial class stokListe : ContentPage
    {
        public ObservableCollection<Urun> UrunListesi { get; set; } = new ObservableCollection<Urun>();

        public stokListe()
        {
            InitializeComponent();
            LoadUrunler();
            UrunListesiView.ItemsSource = UrunListesi;
        }

        public class Urun
        {
            public string Ad { get; set; }
            public int Adet { get; set; }
        }

        private void ekle_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryUrunAdi.Text) || string.IsNullOrWhiteSpace(EntryUrunAdedi.Text))
            {
                DisplayAlert("Hata", "Ürün adý ve adedi boþ býrakýlamaz.", "Tamam");
                return;
            }

            if (!int.TryParse(EntryUrunAdedi.Text, out int adet) || adet <= 0)
            {
                DisplayAlert("Hata", "Ürün adedi pozitif bir sayý olmalýdýr.", "Tamam");
                return;
            }

            UrunListesi.Add(new Urun { Ad = EntryUrunAdi.Text.Trim(), Adet = adet });
            SaveUrunler();
            EntryUrunAdi.Text = string.Empty;
            EntryUrunAdedi.Text = string.Empty;
        }

        private async void ara_Clicked(object sender, EventArgs e)
        {
            string aramaMetni = await DisplayPromptAsync("Ara", "Ürün adý girin:");
            if (string.IsNullOrWhiteSpace(aramaMetni)) return;

            var bulunanUrunler = UrunListesi.Where(u => u.Ad.Contains(aramaMetni, StringComparison.OrdinalIgnoreCase)).ToList();

            if (bulunanUrunler.Count == 0)
            {
                await DisplayAlert("Sonuç", "Hiçbir ürün bulunamadý.", "Tamam");
                return;
            }

            await DisplayAlert("Sonuç", string.Join("\n", bulunanUrunler.Select(u => $"{u.Ad} - {u.Adet}")), "Tamam");
        }

        private void sil_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Urun urun)
            {
                UrunListesi.Remove(urun);
                SaveUrunler();
            }
        }

        private async void guncelle_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Urun urun)
            {
                string yeniAd = await DisplayPromptAsync("Güncelle", "Yeni ürün adýný girin:", initialValue: urun.Ad);
                string yeniAdetStr = await DisplayPromptAsync("Güncelle", "Yeni ürün adedini girin:", initialValue: urun.Adet.ToString());

                if (string.IsNullOrWhiteSpace(yeniAd) || !int.TryParse(yeniAdetStr, out int yeniAdet) || yeniAdet <= 0)
                {
                    await DisplayAlert("Hata", "Geçerli bir ürün adý ve adedi girin.", "Tamam");
                    return;
                }

                urun.Ad = yeniAd;
                urun.Adet = yeniAdet;

                UrunListesiView.ItemsSource = null;
                UrunListesiView.ItemsSource = UrunListesi;
                SaveUrunler();
            }
        }

        private void SaveUrunler()
        {
            var serializedData = string.Join("|", UrunListesi.Select(u => $"{u.Ad},{u.Adet}"));
            Preferences.Set("urunler", serializedData);
        }

        private void LoadUrunler()
        {
            var serializedData = Preferences.Get("urunler", string.Empty);

            if (!string.IsNullOrEmpty(serializedData))
            {
                var urunler = serializedData.Split('|')
                    .Select(data => data.Split(','))
                    .Where(parts => parts.Length == 2 && int.TryParse(parts[1], out _))
                    .Select(parts => new Urun { Ad = parts[0], Adet = int.Parse(parts[1]) })
                    .ToList();

                UrunListesi = new ObservableCollection<Urun>(urunler);
            }
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
