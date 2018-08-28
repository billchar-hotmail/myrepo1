using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureNotesWpfClient.Data;
using System.Collections.ObjectModel;

namespace SecureNotesWpfClient.Models
{
    public class Notebook
    {
        public Notebook()
        {
            Data = new NotebookData();
            Notes = new ObservableCollection<NoteListItem>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ServerId { get; set; }
        public NotebookData Data { get; private set; }
        public ObservableCollection<NoteListItem> Notes { get; private set; }

    }
}
