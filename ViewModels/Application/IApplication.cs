namespace WpfCalava.ViewModels
{
    /// <summary>
    /// Application global status.
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Gets or sets the count of open documents.
        /// </summary>
        int DocumentCount { get; set; }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        ISettings Settings { get; }
    }
}
