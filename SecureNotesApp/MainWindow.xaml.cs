using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Web.Script.Serialization; // Теперь заработает после шага №2

namespace SecureNotesApp
{
    public partial class MainWindow : Window
    {
        private List<Note> _allNotes = new List<Note>();
        private string _masterPassword = "test";
        private string _filePath = "data.bin";
        private JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public MainWindow(string password)
        {
            InitializeComponent(); // Это должно быть ПЕРВОЙ строкой
            _masterPassword = password;
            LoadData();
        }
        private void LoadData()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string encrypted = File.ReadAllText(_filePath);
                    string json = EncryptionService.Decrypt(encrypted, _masterPassword);
                    if (!string.IsNullOrEmpty(json))
                    {
                        _allNotes = _serializer.Deserialize<List<Note>>(json);
                        RefreshList();
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при загрузке данных.");
                }
            }
        }

        private void SaveData()
        {
            string json = _serializer.Serialize(_allNotes);
            string encrypted = EncryptionService.Encrypt(json, _masterPassword);
            File.WriteAllText(_filePath, encrypted);
        }

        private void RefreshList()
        {
            NotesListBox.ItemsSource = null;
            NotesListBox.ItemsSource = _allNotes.OrderByDescending(n => n.UpdatedAt).ToList();
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            var note = new Note { Title = "Новая заметка", Content = "", UpdatedAt = DateTime.Now };
            _allNotes.Add(note);
            RefreshList();
            NotesListBox.SelectedItem = note;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem is Note selected)
            {
                selected.Title = TitleTextBox.Text;
                selected.Content = ContentTextBox.Text;
                selected.UpdatedAt = DateTime.Now;
                SaveData();
                RefreshList();
                MessageBox.Show("Сохранено!");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem is Note selected)
            {
                // Окно подтверждения удаления
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить заметку \"{selected.Title}\"?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _allNotes.Remove(selected);
                    SaveData();
                    RefreshList();
                    TitleTextBox.Clear();
                    ContentTextBox.Clear();
                }
            }
        }

        private void NotesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (NotesListBox.SelectedItem is Note selected)
            {
                TitleTextBox.Text = selected.Title;
                ContentTextBox.Text = selected.Content;
            }
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            NotesListBox.ItemsSource = _allNotes
                .Where(n => n.Title.ToLower().Contains(query) || n.Content.ToLower().Contains(query))
                .ToList();
        }
    }
}