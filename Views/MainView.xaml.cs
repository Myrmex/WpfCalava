using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using WpfCalava.Properties;
using WpfCalava.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace WpfCalava.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            _dockMgr.Loaded += (sender, e) =>
            {
                if (Settings.Default.IsAutoLayoutRestoreEnabled)
                {
                    Width = Settings.Default.MainWidth;
                    Height = Settings.Default.MainHeight;
                    RestoreLayout();
                } //eif
            };
        }

        // this is a workaround similar to that described in 
        // http://stackoverflow.com/questions/17185780/prevent-document-from-closing-in-dockingmanager?rq=1
        // where the main view code directly calls this method handling the docking manager closing event.
        // The scenario is the following: AvalonDock has its document handling mechanism, which should be
        // synchronized with the Caliburn Micro's one. CM is based on a screen conductor; when a screen
        // needs to be closed, the method TryClose is used to close it if possible (i.e. unless a guard
        // method tells the framework that the screen cannot be closed, e.g. because the document is dirty).
        // I found no elegant alternative to this workaround: when AD is closing the document, call
        // the underlying VM guard method and cancel if required; if not cancelled, then AD goes on closing
        // thus firing the DocumentClosed event. Handling this event I can call TryClose on CM so that the
        // viewmodel representing the closed document screen is correctly removed. I can be sure that this
        // will be the case, as TryClose has just been called in handling OnDocumentClosing. I cannot directly
        // call TryClose in OnDocumentClosing, as this would cause null object reference errors in AD.

        private void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            DocumentBase document = e.Document.Content as DocumentBase;
            if (document == null) return;

            e.Cancel = !document.CanClose();
        }

        private void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            DocumentBase document = e.Document.Content as DocumentBase;
            if (document != null) document.Close();
        }

        static private string GetLayoutFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "WpfCalavaLayout.xml");
        }

        private void SaveLayout()
        {
            Settings.Default.MainWidth = ActualWidth;
            Settings.Default.MainHeight = ActualHeight;

            XmlLayoutSerializer serializer = new XmlLayoutSerializer(_dockMgr);
            string sFilePath = GetLayoutFilePath();
            serializer.Serialize(sFilePath);
        }

        private void OnSaveLayoutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveLayout();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void RestoreLayout()
        {
            string sFilePath = GetLayoutFilePath();
            if (!File.Exists(sFilePath)) return;

            // check for well-formedness before restoring:
            // if not well-formed, remove it so we avoid getting stuck.
            try
            {
                XDocument.Load(sFilePath);
            }
            catch (Exception)
            {
                File.Delete(sFilePath);
                throw;
            }

            XmlLayoutSerializer serializer = new XmlLayoutSerializer(_dockMgr);
            serializer.Deserialize(sFilePath);
        }

        private void OnRestoreLayoutClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RestoreLayout();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
    }
}
