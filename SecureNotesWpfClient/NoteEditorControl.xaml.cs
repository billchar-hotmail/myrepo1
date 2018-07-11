using SecureNotesWpfClient.Models;
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
    /// Interaction logic for NoteEditorControl.xaml
    /// </summary>
    public partial class NoteEditorControl : UserControl
    {
        private Note _externalNote;
        private Note _internalNote;
        private bool _inEditMode;
        private bool _showEditButton;
        private bool _showHistoryButton;

        public Note Note
        {
            get { return _externalNote; }
            set {
                _externalNote = value;
                CopyNote(value, _internalNote);
            }
        }

        public bool IsInEditMode
        {
            get { return _inEditMode; }
            set { SetEditMode(value); }
        }

        public bool ShowEditButton
        {
            get { return _showEditButton; }
            set
            {
                editButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                _showEditButton = value;
            }
        }

        public bool ShowHistoryButton
        {
            get { return _showHistoryButton; }
            set
            {
                historyButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                _showHistoryButton = value;
            }
        }

        [Browsable(true)]
        public event EventHandler EditButtonClicked;
        public event EventHandler HistoryButtonClicked;
        public event EventHandler SaveButtonClicked;
        public event EventHandler CancelButtonClicked;

        public NoteEditorControl()
        {
            InitializeComponent();

            _inEditMode = false;
            _externalNote = new Note();
            _internalNote = new Note();
            _showEditButton = editButton.Visibility == Visibility.Visible;
            _showHistoryButton = historyButton.Visibility == Visibility.Visible;

            noteEditGrid.DataContext = _internalNote;
        }

        protected void SetEditMode(bool enabled)
        {
            if (enabled)
            {
                noteTitleTextBox.IsReadOnly = false;
                noteBodyTextBox.IsReadOnly = false;
                editButton.Visibility = Visibility.Collapsed;
                historyButton.Visibility = Visibility.Collapsed;
                saveButton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
                _inEditMode = true;
            }
            else
            {
                noteTitleTextBox.IsReadOnly = true;
                noteBodyTextBox.IsReadOnly = true;
                editButton.Visibility = _showEditButton ? Visibility.Visible : Visibility.Collapsed;
                historyButton.Visibility = _showHistoryButton ? Visibility.Visible : Visibility.Collapsed;
                saveButton.Visibility = Visibility.Collapsed;
                cancelButton.Visibility = Visibility.Collapsed;
                _inEditMode = false;
            }
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

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditButtonClicked != null)
                EditButtonClicked(this, EventArgs.Empty);
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryButtonClicked != null)
                HistoryButtonClicked(this, EventArgs.Empty);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            CopyNote(_externalNote, _internalNote);
            if (CancelButtonClicked != null)
                CancelButtonClicked(this, EventArgs.Empty);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            CopyNote(_internalNote, _externalNote);
            if (SaveButtonClicked != null)
                SaveButtonClicked(this, EventArgs.Empty);
        }

    }
}
