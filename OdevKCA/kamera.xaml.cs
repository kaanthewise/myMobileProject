using System.Collections.ObjectModel;
using System.Text.Json;

namespace OdevKCA;

public partial class kamera : ContentPage
{
    private ObservableCollection<PhotoData> photoList = new ObservableCollection<PhotoData>();
    public kamera()
    {
        InitializeComponent();
        photoListView.ItemsSource = photoList;
        LoadPhotos();
    }

    public class PhotoData
    {
        public string PhotoName { get; set; }
        public string PhotoDate { get; set; }
        public string Location { get; set; }
        public string FilePath { get; set; }
    }

    private async void photoListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedPhoto = e.SelectedItem as PhotoData;
            await DisplayAlert("Fotoðraf Bilgisi",
                $"Adý: {selectedPhoto.PhotoName}\nTarih: {selectedPhoto.PhotoDate}\nKonum: {selectedPhoto.Location}",
                "Tamam");

            deleteButton.IsVisible = true;
            renameButton.IsVisible = true;
        }
        else
        {
            deleteButton.IsVisible = false;
            renameButton.IsVisible = false;
        }

    }

    private async void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchQuery = searchEntry.Text.ToLower();
        var filteredList = photoList.Where(p => p.PhotoName.ToLower().Contains(searchQuery)).ToList();
        photoListView.ItemsSource = filteredList;

    }

    private async void takePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var filePath = photo.FullPath;

                var location = await Geolocation.GetLocationAsync();
                string locationText = location != null
                    ? $"Lat: {location.Latitude}, Lon: {location.Longitude}"
                    : "Bilinmiyor";

                var photoData = new PhotoData
                {
                    PhotoName = Path.GetFileName(photo.FileName),
                    PhotoDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    Location = locationText,
                    FilePath = filePath
                };

                photoList.Add(photoData);
                SavePhotoData();
                photoImage.Source = ImageSource.FromFile(filePath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Fotoðraf çekilemedi: {ex.Message}", "Tamam");
        }

    }

    private async void deleteButton_Clicked(object sender, EventArgs e)
    {
        var selectedPhoto = photoListView.SelectedItem as PhotoData;
        if (selectedPhoto != null)
        {
            photoList.Remove(selectedPhoto);
            SavePhotoData();
            await DisplayAlert("Silindi", "Fotoðraf baþarýyla silindi.", "Tamam");
        }
    }

    private void SavePhotoData()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "photos.json");
        var json = JsonSerializer.Serialize(photoList);
        File.WriteAllText(path, json);
    }

    private void LoadPhotos()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "photos.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var savedPhotos = JsonSerializer.Deserialize<List<PhotoData>>(json);
            if (savedPhotos != null)
            {
                foreach (var photo in savedPhotos)
                {
                    photoList.Add(photo);
                }
            }
        }
    }

    private async void renameButton_Clicked(object sender, EventArgs e)
    {
        var selectedPhoto = photoListView.SelectedItem as PhotoData;
        if (selectedPhoto != null)
        {
            string newName = await DisplayPromptAsync("Yeni Ýsim", "Yeni fotoðraf ismini girin:");
            if (!string.IsNullOrEmpty(newName))
            {
                selectedPhoto.PhotoName = newName;
                SavePhotoData();
                await DisplayAlert("Ýsim Deðiþtirildi", "Fotoðraf ismi baþarýyla deðiþtirildi.", "Tamam");
            }
        }

    }
}