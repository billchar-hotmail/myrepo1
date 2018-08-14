using SecureNotesWpfClient.Data;
using SecureNotesWpfClient.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SecureNotesWpfClient.Models
{
    public class Note : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _id;
        private NoteSyncStatus _syncStatus;
        private NoteMergeStatus _mergeStatus;
        private int _currentVersionNum;
        private NoteData _data;
        private DateTime _createdUTC;
        private DateTime _modifiedUTC;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public NoteSyncStatus SyncStatus
        {
            get { return _syncStatus; }
            set
            {
                _syncStatus = value;
                NotifyPropertyChanged();
            }
        }

        public NoteMergeStatus MergeStatus
        {
            get { return _mergeStatus; }
            set
            {
                _mergeStatus = value;
                NotifyPropertyChanged();
            }
        }

        public int CurrentVersionNum
        {
            get { return _currentVersionNum; }
            set
            {
                _currentVersionNum = value;
                NotifyPropertyChanged();
            }
        }

        public NoteData Data
        {
            get { return _data; }
            private set { _data = value; }
         }

        public DateTime CreatedUTC
        {
            get { return _createdUTC; }
            set
            {
                _createdUTC = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime ModifiedUTC
        {
            get { return _modifiedUTC; }
            set
            {
                _modifiedUTC = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime CreatedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(_createdUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set
            {
                _createdUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId);
                NotifyPropertyChanged();
            }
        }
        public DateTime ModifiedDT
        {
            get { return TimeZoneHelper.ConvertFromUTC(ModifiedUTC, TimeZoneHelper.DefaultTimeZoneId); }
            set
            {
                _modifiedUTC = TimeZoneHelper.ConvertToUTC(value, TimeZoneHelper.DefaultTimeZoneId);
                NotifyPropertyChanged();
            }
        }

        public Note()
        {
            _data = new NoteData();
            _data.PropertyChanged += ((sender, eventArgs) => NotifyPropertyChanged("Data"));
        }

     }
}
