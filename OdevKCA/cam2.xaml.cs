using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OdevKCA;

public partial class cam2 : ContentPage
{
    public List<SavedObject> SavedObjects { get; set; } = new();

    public cam2()
	{
		InitializeComponent();
	}

    // Ana menüye dön
    private async void OnBackToMenuClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    // Resim çek ve kaydet
    private async void OnCaptureAndSaveClicked(object sender, EventArgs e)
    {
        try
        {
            // Kamera eriþimini kontrol et
            if (!MediaPicker.IsCaptureSupported)
            {
                await DisplayAlert("Hata", "Bu cihaz kamera desteklemiyor.", "Tamam");
                return;
            }

            // Gerekli izinleri talep et
            await EnsurePermissionsAsync();

            // Kamera aç
            var photoResult = await MediaPicker.CapturePhotoAsync();

            if (photoResult != null)
            {
                // Fotoðrafý kaydet
                var photoPath = Path.Combine(FileSystem.AppDataDirectory, photoResult.FileName);
                using var stream = await photoResult.OpenReadAsync();
                using var fileStream = File.OpenWrite(photoPath);
                await stream.CopyToAsync(fileStream);

                // Konum bilgisi al
                string locationString = await GetLocationStringAsync();

                // Nesne bilgisi oluþtur
                var newObject = new SavedObject
                {
                    Name = ObjectNameEntry.Text ?? "Bilinmeyen Nesne",
                    PhotoPath = photoPath,
                    Location = locationString,
                    Time = DateTime.Now
                };

                // Listeye ekle
                SavedObjects.Add(newObject);

                // Listeyi güncelle
                UpdateListView();

                await DisplayAlert("Baþarýlý", "Nesne baþarýyla kaydedildi.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }

    // Konum bilgisini elde et
    private async Task<string> GetLocationStringAsync()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();

                return placemark != null
                    ? placemark.Locality ??
                      placemark.SubAdminArea ??
                      placemark.AdminArea ??
                      "Bilinmeyen Yer"
                    : "Bilinmeyen Yer";
            }
            else
            {
                return "Konum alýnamadý";
            }
        }
        catch
        {
            return "Konum alýnamadý";
        }
    }

    // Gerekli izinleri talep et
    private async Task EnsurePermissionsAsync()
    {
        var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
        var locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (cameraStatus != PermissionStatus.Granted || locationStatus != PermissionStatus.Granted)
        {
            throw new Exception("Gerekli izinler verilmedi.");
        }
    }

    // Listeyi güncelle
    private void UpdateListView()
    {
        ObjectCollectionView.ItemsSource = null;
        ObjectCollectionView.ItemsSource = SavedObjects;
    }

    // Listeyi göster
    private void OnListObjectsClicked(object sender, EventArgs e)
    {
        UpdateListView();
    }

    // Arama yap
    private void OnSearchClicked(object sender, EventArgs e)
    {
        var searchQuery = SearchEntry.Text?.ToLower();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var filteredObjects = SavedObjects.FindAll(o => o.Name.ToLower().Contains(searchQuery));
            ObjectCollectionView.ItemsSource = filteredObjects;
        }
        else
        {
            UpdateListView();
        }
    }

    // Kayýtlý nesne modeli
    public class SavedObject
    {
        public string Name { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.Now;
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        System.Environment.Exit(0);

    }
}