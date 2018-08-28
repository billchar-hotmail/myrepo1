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
        //[Browsable(true)]
        //public event EventHandler SaveButtonClicked;

        public ObservableCollection<NoteListItem> Notes
        {
            get { return noteListControl.Notes; }
        }

        public NoteListItem SelectedNote
        {
            get { return noteListControl.SelectedNote; }
        }

        public NotebookEditControl()
        {
            InitializeComponent();
        }

        public void LoadNotebook(Notebook notebook)
        {
            Notes.Clear();
            foreach(var note in notebook.Notes)
            {
                Notes.Add(note);
            }

            var noteId = (notebook.Notes.Count() > 0) ? notebook.Notes.First().Id : string.Empty;
            ViewNoteId(noteId);
        }

        public void ViewNoteId(string noteId)
        {
            leftTabControl.SelectedIndex = 0;
            noteListControl.SelectNoteId(noteId);
        }

        public void AddNote()
        {
            leftTabControl.SelectedIndex = 0;
            noteListControl.SelectNoteId(null);

            var newNote = new NoteListItem()
            {
                Id = String.Empty,
                CurrentVersionNum = 0
            };
            newNote.Data.Title = String.Empty;
            newNote.Data.Body = String.Empty;
            noteEditorControl.Note = newNote;
            noteEditorControl.SetEditMode();
        }

        private void NoteEditor_HistoryButtonClick(object sender, EventArgs e)
        {
            leftTabControl.SelectedIndex = 1;
            noteEditorControl.SetEditMode();
            // HistoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NoteEditor_EditButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.SetEditMode();
        }

        private void NoteEditor_SaveButtonClick(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(noteEditorControl.Note.Id))
            {
                var newNote = noteEditorControl.Note;
                newNote.Id = Guid.NewGuid().ToString("N");
                Notes.Add(newNote);
                noteListControl.SelectNoteId(newNote.Id);
                leftTabControl.SelectedIndex = 0;
            }
            else
            {
                var enableHistory = (noteHistoryControl.Notes.Count() > 0);
                noteEditorControl.SetViewMode(enableEdit: true, enableHistory: enableHistory);
                leftTabControl.SelectedIndex = 0;
            }
            //SaveButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NoteEditor_CancelButtonClick(object sender, EventArgs e)
        {
            if (noteListControl.SelectedNote == null)
            {
                leftTabControl.SelectedIndex = 0;
                noteEditorControl.Note = null;
                noteEditorControl.SetViewMode(false, false);
            }
            else
            {
                leftTabControl.SelectedIndex = 0;
                var enableHistory = (noteHistoryControl.Notes.Count() > 0);
                noteEditorControl.SetViewMode(enableEdit: true, enableHistory: enableHistory);
            }
        }

        private void NoteList_NoteSelectionChanged(object sender, ICollection<NoteListItem> selectedNotes)
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
