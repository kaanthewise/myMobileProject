using System.Collections.ObjectModel;


namespace OdevKCA
{
	public partial class toDO : ContentPage
	{
        private ObservableCollection<string> notes = new ObservableCollection<string>();
        private string selectedNote;

        public toDO()
		{
			InitializeComponent();
            LoadNotes();
        }
        // Yeni not ekleme
        private void OnAddClicked(object sender, EventArgs e)
        {
            string newNote = entryNote.Text;

            if (!string.IsNullOrWhiteSpace(newNote))
            {
                notes.Add(newNote);
                SaveNotes();  // Notlarý kaydet
                entryNote.Text = "";  // Entry'yi temizle
            }
            else
            {
                DisplayAlert("Hata", "Lütfen geçerli bir not girin.", "Tamam");
            }
        }

        // Seçilen notu güncelleme
        private void OnUpdateClicked(object sender, EventArgs e)
        {
            if (selectedNote != null)
            {
                string updatedNote = entryNote.Text;

                if (!string.IsNullOrWhiteSpace(updatedNote))
                {
                    int index = notes.IndexOf(selectedNote);
                    notes[index] = updatedNote;
                    SaveNotes();  // Notlarý kaydet
                    entryNote.Text = "";  // Entry'yi temizle
                    selectedNote = null;  // Seçilen notu sýfýrla
                }
                else
                {
                    DisplayAlert("Hata", "Lütfen geçerli bir not girin.", "Tamam");
                }
            }
            else
            {
                DisplayAlert("Hata", "Güncellenecek bir not seçin.", "Tamam");
            }
        }

        // Seçilen notu silme
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (selectedNote != null)
            {
                notes.Remove(selectedNote);
                SaveNotes();  // Notlarý kaydet
                entryNote.Text = "";  // Entry'yi temizle
                selectedNote = null;  // Seçilen notu sýfýrla
            }
            else
            {
                DisplayAlert("Hata", "Silinecek bir not seçin.", "Tamam");
            }
        }

        // Notlarý arama
        private void OnSearchClicked(object sender, EventArgs e)
        {
            string searchQuery = entryNote.Text;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var searchResults = notes.Where(n => n.Contains(searchQuery)).ToList();
                listViewNotes.ItemsSource = searchResults;  // Arama sonuçlarýný göster
            }
            else
            {
                listViewNotes.ItemsSource = notes;  // Arama boþsa tüm notlarý göster
            }
        }

        // ListView'den bir not seçildiðinde
        private void OnNoteSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedNote = e.SelectedItem as string;
            if (selectedNote != null)
            {
                entryNote.Text = selectedNote;  // Seçilen notu entry'ye yaz
            }
            else
            {
                entryNote.Text = "";  // Seçim yoksa Entry'yi temizle
            }
        }

        // Notlarý kaydetme (Preferences)
        private void SaveNotes()
        {
            Preferences.Set("notes", string.Join(",", notes));  // Notlarý virgülle ayýrarak sakla
        }

        // Notlarý yükleme (Preferences)
        private void LoadNotes()
        {
            var savedNotes = Preferences.Get("notes", string.Empty);
            if (!string.IsNullOrEmpty(savedNotes))
            {
                notes = new ObservableCollection<string>(savedNotes.Split(',').ToList());  // Virgülle ayrýlmýþ notlarý yükle
            }
            listViewNotes.ItemsSource = notes;  // ListView'i güncelle
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




