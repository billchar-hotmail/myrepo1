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
            Notes = new List<Note>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ServerId { get; set; }
        public NotebookData Data { get; set; }
        public List<Note> Notes { get; set; }

    }
}
