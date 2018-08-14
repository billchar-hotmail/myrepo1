using SecureNotesWpfClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class NotebookEditControl : UserControl
    {
        private ICollection<Note> _notes;

        //[Browsable(true)]
        //public event EventHandler SaveButtonClicked;

        public ICollection<Note> Notes
        {
            get { return _notes; }
            set { SetNotes(value); }
        }

        public Note SelectedNote
        {
            get { return noteListControl.SelectedNote; }
            set { noteListControl.SelectedNote = value; }
        }

        public NotebookEditControl()
        {
            InitializeComponent();

            _notes = new Collection<Note>();
            noteListControl.Notes = _notes;
        }

        protected void SetNotes(ICollection<Note> notes)
        {
            _notes = notes;
            noteListControl.Notes = _notes;
            noteListControl.SelectNoteId(null);
            noteEditorControl.Note = null;
            noteEditorControl.SetViewMode(enableEdit: false, enableHistory: false);
        }

        public void SelectNoteId(string noteId)
        {
            noteListControl.SelectNoteId(noteId);
        }

        private void NoteEditor_HistoryButtonClick(object sender, EventArgs e)
        {
           // HistoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NoteEditor_EditButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.SetEditMode();
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
            //SaveButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NoteEditor_CancelButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.SetViewMode(true, true);

        }

        public void AddNote()
        {
            noteListControl.SelectNoteId(null);
            var newNote = new Note()
            {
                Id = String.Empty,
                CurrentVersionNum = 0
            };
            newNote.Data.Title = String.Empty;
            newNote.Data.Body = String.Empty;
            noteEditorControl.Note = newNote;
            noteEditorControl.SetEditMode();

        }

        private void NoteList_NoteSelectionChanged(object sender, ICollection<Note> selectedNotes)
        {
            if (selectedNotes.Count == 1)
            {
                var note = selectedNotes.First();
                noteHistoryControl.LoadNoteId(note.Id);
                var enableHistory = (noteHistoryControl.Notes.Count() > 0);
                noteEditorControl.Note = note;
                noteEditorControl.SetViewMode(enableEdit: true, enableHistory: enableHistory);
            }
            else
            {
                noteHistoryControl.LoadNoteId(null);
                noteEditorControl.Note = null;
                noteEditorControl.SetViewMode(enableEdit: false, enableHistory: false);
            }

        }

    }
}
