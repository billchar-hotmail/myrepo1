using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Data
{
    public class NoteData : INotifyPropertyChanged
    {
        private string _title;
        private string _body;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public string Title {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        public string Body {
            get { return _body; }
            set
            {
                _body = value;
                NotifyPropertyChanged();
            }
        }

        public string AsString
        {
            get { return GetAsString(); }
            set { SetAsString(value); }
        }


        private string GetAsString()
        {
            return Title + "\n" + Body;
        }
        
        private void SetAsString(string data)
        {
            var pos = data.IndexOf("\n");
            Title = data.Substring(0, pos);
            Body = data.Substring(pos + 1);
        }
    }
}
