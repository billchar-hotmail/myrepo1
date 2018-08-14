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
        private List<NoteVersion> _noteHistory;

        public IEnumerable<NoteVersion> Notes
        {
            get { return _noteHistory; }
        }
        
        public NoteHistoryControl()
        {
            InitializeComponent();
            _notesService = new NotesService();
            _noteHistory = new List<NoteVersion>();
            //this.DataContext = this;
        }

        public int LoadNoteId(string noteId)
        {
            _noteHistory = _notesService.GetNoteHistory(noteId);
            versionComboBox.ItemsSource = _noteHistory;

            // Add extra items
            _noteHistory.Add(new NoteVersion()
            {
                NoteId = "1",
                VersionNum = 2,
                CreatedUTC = DateTime.UtcNow,
                Data = new Data.NoteData()
                {
                    Title = "Test 123",
                    Body = "Test body 123"
                }
            });
            _noteHistory.Add(new NoteVersion()
            {
                NoteId = "1",
                VersionNum = 3,
                CreatedUTC = DateTime.UtcNow,
                Data = new Data.NoteData()
                {
                    Title = "Test 1234",
                    Body = "Test body 1234"
                }
            });

            versionComboBox.SelectedIndex = 0;
            return _noteHistory.Count;
        }

        protected void LoadText(int index)
        {
            if (index < _noteHistory.Count)
                noteBodyTextBox.Text = _noteHistory[index].Data.Body;
            else
                noteBodyTextBox.Text = string.Empty;
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
