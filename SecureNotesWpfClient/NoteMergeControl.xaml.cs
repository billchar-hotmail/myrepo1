using System;
using System.Collections.Generic;
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
    /// Interaction logic for NoteMergeControl.xaml
    /// </summary>
    public partial class NoteMergeControl : UserControl
    {
        public NoteMergeControl()
        {
            InitializeComponent();
        }

        private void NoteEditor_SaveButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.IsInEditMode = false;
        }

        private void NoteEditor_CancelButtonClick(object sender, EventArgs e)
        {
            noteEditorControl.IsInEditMode = false;
        }


    }
}
