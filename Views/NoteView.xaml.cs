using System.Windows;
using System.Windows.Controls;

namespace WpfCalava.Views
{
    public partial class NoteView : UserControl
    {
        public NoteView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _txtText.Focus();
        }
    }
}
