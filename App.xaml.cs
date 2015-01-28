using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace WpfCalava
{
    public partial class App : Application
    {
        public const string APP_NAME = "WpfCalava";

        private static void InitCulture()
        {
            Thread.CurrentThread.CurrentCulture = 
                new CultureInfo(WpfCalava.Properties.Settings.Default.Culture);
            
            Thread.CurrentThread.CurrentUICulture =
                new CultureInfo(WpfCalava.Properties.Settings.Default.Culture);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage
                    (WpfCalava.Properties.Settings.Default.Culture)));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // handle unhandled exceptions from threads other than the UI thread
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            InitCulture();

            base.OnStartup(e);

            // you could handle a login process here e.g.
            // bool b = await Login();
            // if (!b) Current.Shutdown();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // http://www.codeproject.com/Articles/90866/Unhandled-Exception-Handler-For-WPF-Applications

            // TODO: globally handle exception
            MessageBox.Show(((Exception) e.ExceptionObject).Message);
        }

        /// <summary>
        /// Handle any unhandled exception raised on the UI thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // http://www.codeproject.com/Articles/90866/Unhandled-Exception-Handler-For-WPF-Applications

            e.Handled = true;
            // TODO: globally handle exception
            MessageBox.Show(e.Exception.Message);
        }
    }
}
