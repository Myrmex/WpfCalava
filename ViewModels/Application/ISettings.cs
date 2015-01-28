namespace WpfCalava.ViewModels
{
    /// <summary>
    /// Application settings.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets or sets the UI culture.
        /// </summary>
        string Culture { get; set; }

        /// <summary>
        /// Gets or sets the maximum open documents count.
        /// </summary>
        int MaxOpenDocuments { get; set; }
    }
}
