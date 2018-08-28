using SecureNotesWpfClient.Models;
using SecureNotesWpfClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecureNotesWpfClient
{
    /// <summary>
    /// Interaction logic for NoteHistoryControl.xaml
    /// </summary>
    public partial class NoteHistoryControl : UserControl
    {
        private NotesService _notesService;
        private ObservableCollection<NoteVersion> _noteHistory;

        public IEnumerable<NoteVersion> Notes
        {
            get { return _noteHistory; }
        }
        
        public NoteHistoryControl()
        {
            InitializeComponent();
            _notesService = new NotesService();
            _noteHistory = new ObservableCollection<NoteVersion>();
            versionComboBox.ItemsSource = _noteHistory;
        }

        public int LoadNoteId(string noteId)
        {
            var notes = _notesService.GetNoteHistory(noteId);
            _noteHistory.Clear();
            foreach(var note in notes)
            {
                _noteHistory.Add(note);
            }
            versionComboBox.SelectedIndex = 0;
            return _noteHistory.Count;
        }

        private void versionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var note = (NoteVersion)e.AddedItems[0];
                noteBodyTextBox.Text = note.Data.Body;
            }
            else
                noteBodyTextBox.Text = string.Empty;
        }
    }
}
