using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SecureNotesWpfClient
{
    public class AppConfig
    {
        public const string AppVersion = "1.0";
        public static string DbFilename { get; private set; }
        public static string HostServer { get; private set; }
        public static string ConnectionString { get; private set; }

        public static void Init()
        {
            // DbFilename = System.Reflection.Assembly.GetEntryAssembly().Location + "\\Settings\\UserSettings\\data.db";
            //DbFilename = "c:\\temp\\data.db";
            DbFilename = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\SecureNotes\\data.db";
            HostServer = "localhost";

            var regKey = Registry.LocalMachine.OpenSubKey(@"Software\CharboTech\SecureNotes");
            if (regKey != null)
            {
                string dbFilename = (string)regKey.GetValue("DbFilename");
                if (!string.IsNullOrEmpty(dbFilename))
                    DbFilename = dbFilename;

                string hostServer = (string)regKey.GetValue("HostServer");
                if (!string.IsNullOrEmpty(hostServer))
                    HostServer = hostServer;
            }
            ConnectionString = "Data Source=" + DbFilename;
        }

    }
}
