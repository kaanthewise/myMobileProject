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
                SaveNotes();  // Notlar� kaydet
                entryNote.Text = "";  // Entry'yi temizle
            }
            else
            {
                DisplayAlert("Hata", "L�tfen ge�erli bir not girin.", "Tamam");
            }
        }

        // Se�ilen notu g�ncelleme
        private void OnUpdateClicked(object sender, EventArgs e)
        {
            if (selectedNote != null)
            {
                string updatedNote = entryNote.Text;

                if (!string.IsNullOrWhiteSpace(updatedNote))
                {
                    int index = notes.IndexOf(selectedNote);
                    notes[index] = updatedNote;
                    SaveNotes();  // Notlar� kaydet
                    entryNote.Text = "";  // Entry'yi temizle
                    selectedNote = null;  // Se�ilen notu s�f�rla
                }
                else
                {
                    DisplayAlert("Hata", "L�tfen ge�erli bir not girin.", "Tamam");
                }
            }
            else
            {
                DisplayAlert("Hata", "G�ncellenecek bir not se�in.", "Tamam");
            }
        }

        // Se�ilen notu silme
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (selectedNote != null)
            {
                notes.Remove(selectedNote);
                SaveNotes();  // Notlar� kaydet
                entryNote.Text = "";  // Entry'yi temizle
                selectedNote = null;  // Se�ilen notu s�f�rla
            }
            else
            {
                DisplayAlert("Hata", "Silinecek bir not se�in.", "Tamam");
            }
        }

        // Notlar� arama
        private void OnSearchClicked(object sender, EventArgs e)
        {
            string searchQuery = entryNote.Text;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var searchResults = notes.Where(n => n.Contains(searchQuery)).ToList();
                listViewNotes.ItemsSource = searchResults;  // Arama sonu�lar�n� g�ster
            }
            else
            {
                listViewNotes.ItemsSource = notes;  // Arama bo�sa t�m notlar� g�ster
            }
        }

        // ListView'den bir not se�ildi�inde
        private void OnNoteSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedNote = e.SelectedItem as string;
            if (selectedNote != null)
            {
                entryNote.Text = selectedNote;  // Se�ilen notu entry'ye yaz
            }
            else
            {
                entryNote.Text = "";  // Se�im yoksa Entry'yi temizle
            }
        }

        // Notlar� kaydetme (Preferences)
        private void SaveNotes()
        {
            Preferences.Set("notes", string.Join(",", notes));  // Notlar� virg�lle ay�rarak sakla
        }

        // Notlar� y�kleme (Preferences)
        private void LoadNotes()
        {
            var savedNotes = Preferences.Get("notes", string.Empty);
            if (!string.IsNullOrEmpty(savedNotes))
            {
                notes = new ObservableCollection<string>(savedNotes.Split(',').ToList());  // Virg�lle ayr�lm�� notlar� y�kle
            }
            listViewNotes.ItemsSource = notes;  // ListView'i g�ncelle
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




