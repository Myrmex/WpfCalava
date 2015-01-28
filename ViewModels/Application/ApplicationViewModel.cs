using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using WpfCalava.Messages;

namespace WpfCalava.ViewModels
{
    /// <summary>
    /// Application status. Whenever a property of this VM changes and your VMs
    /// need to respond to this change, you can fire a 
    /// <see cref="ApplicationStateChangedMessage"/> message.
    /// </summary>
    [Export(typeof(IApplication))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ApplicationViewModel : PropertyChangedBase, IApplication
    {
        private readonly IEventAggregator _events;
        private readonly AppSettings _settings;
        private int _nDocCount;

        /// <summary>
        /// Gets or sets the count of open documents.
        /// </summary>
        public int DocumentCount
        {
            get { return _nDocCount; }
            set
            {
                _nDocCount = value;
                NotifyOfPropertyChange(() => DocumentCount);
            }
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public ISettings Settings
        {
            get { return _settings; }
        }

        [ImportingConstructor]
        public ApplicationViewModel(IEventAggregator events)
        {
            if (events == null) throw new ArgumentNullException("events");
            _events = events;
            _settings = new AppSettings();
        }
    }
}
