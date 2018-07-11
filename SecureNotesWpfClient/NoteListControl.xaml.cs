using SecureNotesWpfClient.Models;
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
    /// Interaction logic for NoteListControl.xaml
    /// </summary>
    public partial class NoteListControl : UserControl
    {
        private ICollection<Note> _notes;

        public NoteListControl()
        {
            InitializeComponent();
            //noteEditorControl.EditButtonClicked += NoteEditor_EditButtonClick;
            _notes = new Collection<Note>();
            lstViewNotes.ItemsSource = _notes;
        }

        public ICollection<Note> Notes
        {
            get
            {
                return _notes; 
            }
            set
            {
                _notes = value;
                lstViewNotes.ItemsSource = value;
            }
        }

        public bool SelectNoteId(string noteId)
        {
            var note = _notes.Where(x => x.Id == noteId).FirstOrDefault();
            if (note == null)
                return false;
            lstViewNotes.SelectedItems.Add(note);
            return true;
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lstViewNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = lstViewNotes.SelectedItems;
            if (items.Count == 1)
            {
                noteEditorControl.Note = (Note)items[0];
                noteEditorControl.ShowEditButton = true;
                noteEditorControl.ShowHistoryButton = true;
            }
            else
            {
                noteEditorControl.Note = null;
                noteEditorControl.ShowEditButton = false;
                noteEditorControl.ShowHistoryButton = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //noteTitleTextBox.Text = SelectedNote.Data.Title;  // noteBodyTextBox.Text;
        }

        private void NoteEditor_HistoryButtonClick(object sender, EventArgs e)
        {
            //noteEditorControl.IsInEditMode = true;
        }

        private void NoteEditor_EditButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.IsInEditMode = true;
        }

        private void NoteEditor_SaveButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.IsInEditMode = false;
            if(String.IsNullOrEmpty(noteEditorControl.Note.Id))
            {
                var newNote = noteEditorControl.Note;
                newNote.Id = Guid.NewGuid().ToString("N");
                _notes.Add(newNote);
                SelectNoteId(newNote.Id);
            }
        }

        private void NoteEditor_CancelButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.IsInEditMode = false;
        }

        public void AddNote()
        {
            lstViewNotes.SelectedIndex = -1;
            var newNote = new Note()
            {
                Id = String.Empty,
                CurrentVersionNum = 0
            };
            newNote.Data.Title = String.Empty;
            newNote.Data.Body = String.Empty;
            noteEditorControl.Note = newNote;
            noteEditorControl.IsInEditMode = true;
        }



    }
}
