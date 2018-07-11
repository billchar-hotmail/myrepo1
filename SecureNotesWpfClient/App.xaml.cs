﻿using SecureNotesWpfClient.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SecureNotesWpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.Init();
            DatabaseDefinition.CreateDatabase();
            DatabaseDefinition.SeedDatabase();
        }
    }
}
