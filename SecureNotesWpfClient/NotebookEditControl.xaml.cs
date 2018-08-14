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

        public ObservableCollection<Note> Notes { get; private set; }

        //public static readonly DependencyProperty LabelProperty =
        //    DependencyProperty.Register("Label", typeof(string),
        //    typeof(FieldUserControl), new PropertyMetadata(""));



        public Note SelectedNote
        {
            get { return noteListControl.SelectedNote; }
            set { noteListControl.SelectedNote = value; }
        }

        public NotebookEditControl()
        {
            InitializeComponent();

            Notes = new ObservableCollection<Note>();
            noteListControl.Notes = Notes;
        }

        protected void SetNotes(ICollection<Note> notes)
        {
            Notes = notes;
            noteListControl.Notes = Notes;
            var noteId = (notes.Count() > 0) ? notes.First().Id : string.Empty;
            SelectNoteId(noteId, false);

            noteListControl.SelectNoteId(null);
            noteEditorControl.Note = null;
            noteEditorControl.SetViewMode(enableEdit: false, enableHistory: false);
        }

        public void SelectNoteId(string noteId, bool editMode = false)
        {
            leftTabControl.SelectedIndex = 0;
            noteListControl.SelectNoteId(noteId);
            //noteHistoryControl.LoadNoteId(noteId);
            
        }
        public void AddNote()
        {
            leftTabControl.SelectedIndex = 0;
            noteHistoryControl.LoadNoteId(null);
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
                _notes.Add(newNote);
                SelectNoteId(newNote.Id);
            }
            var enableHistory = (noteHistoryControl.Notes.Count() > 0);
            noteEditorControl.SetViewMode(enableEdit: true, enableHistory: false);

            //SaveButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void NoteEditor_CancelButtonClick(object sender, EventArgs e)
        {
            leftTabControl.SelectedIndex = 0;
            var enableHistory = (noteHistoryControl.Notes.Count() > 0);
            noteEditorControl.SetViewMode(enableEdit: true, enableHistory: enableHistory);
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
