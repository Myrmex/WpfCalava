using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using WpfCalava.Messages;

namespace WpfCalava.ViewModels
{
    [Export(typeof(IShell))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class MainViewModel : Conductor<IScreen>.Collection.OneActive, IShell,
        IHandle<ApplicationStateChangedMessage>
    {
        private readonly IEventAggregator _events;
        private readonly IWindowManager _window;
        private readonly IApplication _application;
        private readonly Regex _rNumericSuffix;

        #region Properties
        /// <summary>
        /// Gets the tools panes.
        /// </summary>
        public BindableCollection<ToolBase> Tools { get; private set; }

        /// <summary>
        /// Gets the document panes.
        /// </summary>
        public BindableCollection<DocumentBase> Documents { get; private set; }

        /// <summary>
        /// Gets the settings. Useful to bind UI elements to some setting.
        /// </summary>
        public ISettings Settings
        {
            get { return _application.Settings; }
        }

        public WordCounterViewModel WordCounter { get; set; }
        #endregion

        [ImportingConstructor]
        internal MainViewModel(IEventAggregator events,
            IWindowManager window,
            IApplication application,
            WordCounterViewModel wordCounter)
        {
            if (events == null) throw new ArgumentNullException("events");
            if (window == null) throw new ArgumentNullException("window");
            if (application == null) throw new ArgumentNullException("application");
            if (wordCounter == null) throw new ArgumentNullException("wordCounter");

            _events = events;
            _window = window;
            _application = application;

            // we expose this tool as a property as we want to bind the View menu
            // to its IsVisible property. Otherwise adding it to the tools collection
            // would be enough.
            WordCounter = wordCounter;

            Tools = new BindableCollection<ToolBase>
            {
                WordCounter
            };

            Documents = new BindableCollection<DocumentBase>();
            // @@hack for synching the documents collection
            Items.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in args.OldItems.OfType<DocumentBase>())
                        Documents.Remove(item);
                }
            };

            _rNumericSuffix = new Regex(@"(\d+)$");
            DisplayName = App.APP_NAME;

            _events.Subscribe(this);
        }

        private int ParseNameNumber(string sName)
        {
            Match m = _rNumericSuffix.Match(sName);
            return (m.Success ? Int32.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture) : 0);
        }

        private int GetNextDocumentNumber(Type t)
        {
            if (Items.Count == 0) return 1;

            var filtered = Items.Where(i => i.GetType() == t);
            return (filtered.Any()
                ? 1 + filtered.Max(i => ParseNameNumber(i.DisplayName))
                : 1);
        }

        private void AddDocument(string id)
        {
            // just activate an already opened item
            var documents = Items.OfType<DocumentBase>();
            DocumentBase document = documents.FirstOrDefault(e => e.Id == id);
            if (document != null)
            {
                ActivateItem(document);
                return;
            } //eif

            // Else create a new note and activate it.
            // In this example we deal with a single document type, but you
            // could easily add some logic to add different types.
            // TODO: add your new-document logic here
            NoteViewModel vm = IoC.Get<NoteViewModel>();
            vm.DisplayName = vm.DisplayName + " - " + GetNextDocumentNumber(vm.GetType());
            vm.Id = id;
            Documents.Add(vm);
            ActivateItem(vm);
        }

        public void AddDocument()
        {
            // here we just add a new document
            // TODO: add your document retrieval logic here
            AddDocument(Guid.NewGuid().ToString("N"));
        }

        public void Handle(ApplicationStateChangedMessage message)
        {
            // TODO react to state changes if required
        }
    }
}
