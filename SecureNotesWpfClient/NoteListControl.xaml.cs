﻿using SecureNotesWpfClient.Models;
using System;
using System.Collections;
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
    public partial class NoteListControl : UserControl
    {
        private ObservableCollection<NoteListItem> _notes;

        public delegate void NoteSelectionChangedHandler(object sender, ICollection<NoteListItem> selectedNotes);

        [Browsable(true)]
        public event NoteSelectionChangedHandler NoteSelectionChanged;

        public NoteListControl()
        {
            InitializeComponent();
            //noteEditorControl.EditButtonClicked += NoteEditor_EditButtonClick;
            _notes = new ObservableCollection<NoteListItem>();
            lstViewNotes.ItemsSource = _notes;
        }

        public ObservableCollection<NoteListItem> Notes
        {
            get { return _notes; }
        }

        public NoteListItem SelectedNote
        {
            get { return (NoteListItem)lstViewNotes.SelectedItem; }
            set { SelectNoteId(value?.Id); }
        }

        public void SelectNoteId(string noteId)
        {
            var note = _notes.Where(x => x.Id == noteId).FirstOrDefault();
            if (note == null)
                lstViewNotes.UnselectAll();
            else
                lstViewNotes.SelectedItem = note;
        }

        public void AddNote(NoteListItem note)
        {
            _notes.Add(note);
            SelectNoteId(note.Id);
        }

        public void UpdateNote(NoteListItem note)
        {
            var item = _notes.Where(x => x.Id == note.Id).FirstOrDefault();
            if (item != null)
            {
                item.SyncStatus = note.SyncStatus;
                item.Merged = note.Merged;
                item.CurrentVersionNum = note.CurrentVersionNum;
                item.CreatedUTC = note.CreatedUTC;
                item.ModifiedUTC = note.ModifiedUTC;
                item.Data.Title = note.Data.Title;
                item.Data.Body = note.Data.Body;
            }
        }

        private void lstViewNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteSelectionChanged != null)
            {
                var list = new List<NoteListItem>();
                var items = lstViewNotes.SelectedItems;
                foreach (var item in items)
                {
                    list.Add(item as NoteListItem);
                }
                NoteSelectionChanged.Invoke(this, list);
            }
        }



    }
}
