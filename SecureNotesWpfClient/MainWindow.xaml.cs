using SecureNotesWpfClient.ApiModels;
using SecureNotesWpfClient.Models;
using SecureNotesWpfClient.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotesService _notesService;

        public Notebook CurrentNotebook;

        public MainWindow()
        {
            InitializeComponent();

            _notesService = new NotesService();

            CurrentNotebook = _notesService.GetNotebook("1");
            //notesList.DataContext = CurrentNotebook.Notes;
            notebookEditControl.Notes = CurrentNotebook.Notes;
        }

        private void CommonCommanBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void localTimeButtonClick(object sender, RoutedEventArgs e)
        {
            //textBox.Text = DateTime.Now.ToString();
            //TestUC1.FileName = textBox.Text;
        }



        private void serverTimeButtonClick(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:58454");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = client.GetAsync("/api/time/1").Result;
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var data = response.Content.ReadAsByteArrayAsync().Result;
                var dataStr = System.Text.Encoding.Default.GetString(data);
                var model = response.Content.ReadAsAsync<TimeApiModel>().Result;
                //if (string.IsNullOrEmpty(model.DateStr) || string.IsNullOrEmpty(model.TimeStr))
                //    textBox.Text = "No data retrieved";
                //else
                //    textBox.Text = model.TimeStr + " " + model.DateStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ClientError: " + ex.Message);
            }
        }

        private void serverSecureButtonClick(object sender, RoutedEventArgs e)
        {
            //WebRequestHandler handler = new WebRequestHandler();

            HttpClient client = new HttpClient();
            
            client.BaseAddress = new Uri("http://localhost:58454");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = client.GetAsync("/api/stime/1").Result;
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var data = response.Content.ReadAsByteArrayAsync().Result;
                var model = response.Content.ReadAsAsync<TimeApiModel>().Result;
                //if (string.IsNullOrEmpty(model.DateStr) || string.IsNullOrEmpty(model.TimeStr))
                //    textBox.Text = "No data retrieved";
                //else
                //    textBox.Text = model.TimeStr + " " + model.DateStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ClientError: " + ex.Message);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (toolsMenuItem.Visibility == Visibility.Visible)
                toolsMenuItem.Visibility = Visibility.Collapsed;
            else
                toolsMenuItem.Visibility = Visibility.Visible;

            //if (TabControl1.SelectedIndex == 0)
            //    TabControl1.SelectedIndex = 1;
            //else
            //    TabControl1.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //CreateDatabase();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                //PopulateDatabase();
            }
            catch (Exception ex)
            {
                //TestUC1.FileName = ex.Message;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
           // QueryDatabase();
        }

        private void test1MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var win1 = new AccountSetupWindow();
            win1.Owner = this;
            win1.ShowDialog();

        }

        private void test2MenuItem_Click(object sender, RoutedEventArgs e)
        {
            

            //TestUC1.LoadNotebook(CurrentNotebook;
        }

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            notebookEditControl.AddNote();
        }


        private void NotebookEdit_SaveNote(object sender, EventArgs e)
        {
            var note = notebookEditControl.SelectedNote;
            //notebookEditControl.UpdateNote(note);
            mainTabControl.SelectedIndex = 0;
        }
    }

}
