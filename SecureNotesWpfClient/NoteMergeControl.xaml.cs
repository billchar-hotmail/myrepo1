using SecureNotesWpfClient.Models;
using SecureNotesWpfClient.Services;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for NoteMergeControl.xaml
    /// </summary>
    public partial class NoteMergeControl : UserControl
    {
        private NotesService _notesService;
        private List<NoteVersion> _noteHistory;
        private Note _externalNote;

        [Browsable(true)]
        public event EventHandler SaveButtonClicked;
        public event EventHandler CancelButtonClicked;

        public NoteMergeControl()
        {
            InitializeComponent();

            _notesService = new NotesService();
            _externalNote = new Note();
            _noteHistory = new List<NoteVersion>();
        }

        public Note Note
        {
            get { return _externalNote; }
            set { SetNote(value); }
        }

        protected void SetNote(Note note)
        {
            _externalNote = note;
            noteEditorControl.Note = note;
            noteEditorControl.IsInEditMode = true;
            noteHistoryControl.LoadNoteId(note.Id);
        }


        private void NoteEditor_SaveButtonClick(object sender, EventArgs e)
        {
            CopyNote(noteEditorControl.Note, _externalNote);
            SaveButtonClicked?.Invoke(this, EventArgs.Empty);
            noteEditorControl.IsInEditMode = false;
        }

        private void NoteEditor_CancelButtonClick(object sender, EventArgs e)
        {
            CancelButtonClicked?.Invoke(this, EventArgs.Empty);
            noteEditorControl.IsInEditMode = false;
        }

        protected void CopyNote(Note source, Note dest)
        {
            if (dest == null) return;
            if (source == null)
            {
                ClearNote(dest);
            }
            else
            {
                dest.Data.Title = source.Data.Title;
                dest.Data.Body = source.Data.Body;
            }
        }

        protected void ClearNote(Note note)
        {
            if (note == null) return;
            note.Data.Title = "";
            note.Data.Body = "";
        }



    }
}
