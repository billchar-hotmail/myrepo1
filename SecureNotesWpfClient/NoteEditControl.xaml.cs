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
    public partial class NoteEditControl : UserControl
    {
        private NoteListItem _externalNote;
        private NoteListItem _internalNote;
        private bool _inEditMode;

        public NoteListItem Note
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
        }

        [Browsable(true)]
        public event EventHandler EditButtonClicked;
        public event EventHandler HistoryButtonClicked;
        public event EventHandler SaveButtonClicked;
        public event EventHandler CancelButtonClicked;

        public NoteEditControl()
        {
            InitializeComponent();

            _inEditMode = false;
            _externalNote = new NoteListItem();
            _internalNote = new NoteListItem();
            noteEditGrid.DataContext = _internalNote;
        }

        public void SetViewMode(bool enableEdit, bool enableHistory)
        {
            noteTitleTextBox.IsReadOnly = true;
            noteBodyTextBox.IsReadOnly = true;
            editButton.Visibility = Visibility.Visible;
            editButton.IsEnabled = enableEdit;
            historyButton.Visibility = Visibility.Visible;
            historyButton.IsEnabled = enableHistory;
            saveButton.Visibility = Visibility.Collapsed;
            cancelButton.Visibility = Visibility.Collapsed;
            _inEditMode = false;
        }

        public void SetEditMode()
        {
            noteTitleTextBox.IsReadOnly = false;
            noteBodyTextBox.IsReadOnly = false;
            editButton.Visibility = Visibility.Collapsed;
            historyButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            _inEditMode = true;
        }

        protected void CopyNote(NoteListItem source, NoteListItem dest)
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

        protected void ClearNote(NoteListItem note)
        {
            if (note == null) return;
            note.Data.Title = "";
            note.Data.Body = "";
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            EditButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            CopyNote(_externalNote, _internalNote);
            CancelButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            CopyNote(_internalNote, _externalNote);
            SaveButtonClicked?.Invoke(this, EventArgs.Empty);
        }

    }
}
