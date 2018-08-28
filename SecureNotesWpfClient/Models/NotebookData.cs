using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Models
{
    public class NotebookData
    {
        public string HashKey { get; set; }

        public string AsString
        {
            get { return GetAsString(); }
            set { SetAsString(value); }
        }

        private string GetAsString()
        {
            return HashKey;
        }

        private void SetAsString(string data)
        {
            HashKey = data;
        }
    }
}
