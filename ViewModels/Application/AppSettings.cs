using Caliburn.Micro;

namespace WpfCalava.ViewModels
{
    /// <summary>
    /// Wrapper for application settings.
    /// </summary>
    public sealed class AppSettings : PropertyChangedBase, ISettings
    {
        /// <summary>
        /// Gets or sets the UI culture.
        /// </summary>
        public string Culture
        {
            get { return Properties.Settings.Default.Culture; }
            set
            {
                if (Properties.Settings.Default.Culture == value) return;
                Properties.Settings.Default.Culture = value ?? "it-IT";
                NotifyOfPropertyChange(() => Culture);
            }
        }

        /// <summary>
        /// Gets or sets the maximum open documents count.
        /// </summary>
        public int MaxOpenDocuments
        {
            get { return Properties.Settings.Default.MaxOpenDocuments; }
            set
            {
                if (Properties.Settings.Default.MaxOpenDocuments == value) return;
                Properties.Settings.Default.MaxOpenDocuments = value;
                NotifyOfPropertyChange(() => MaxOpenDocuments);
            }
        }
    }
}
