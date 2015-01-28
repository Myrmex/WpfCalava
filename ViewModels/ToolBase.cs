using System.Windows.Media;
using Caliburn.Micro;

namespace WpfCalava.ViewModels
{
    /// <summary>
    /// A generic base class for Caliburn screens to be used with AvalonDock.
    /// </summary>
    /// <remarks>Cfr. https://avalondock.codeplex.com/SourceControl/latest#Version2.0/AvalonDock.MVVMTestApp/PaneViewModel.cs and
    /// http://stackoverflow.com/questions/21692918/does-the-dockingmanager-come-with-a-built-in-method-for-handling-anchorables .</remarks>
    public abstract class ToolBase : Screen
    {
        private bool _bIsSelected;
        private bool _bIsVisible;

        /// <summary>
        /// Gets or sets the icon source.
        /// </summary>
        public ImageSource IconSource { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// This is an AD 2-way bound property and I am not sure of its usefulness with CM.
        /// </summary>
        public bool IsSelected
        {
            get { return _bIsSelected; }
            set
            {
                if (value.Equals(_bIsSelected)) return;
                _bIsSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible. This should
        /// be bound to the Visibility property of the tool controls.
        /// </summary>
        public bool IsVisible
        {
            get { return _bIsVisible; }
            set
            {
                if (value.Equals(_bIsVisible)) return;
                _bIsVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBase"/> class.
        /// </summary>
        protected ToolBase()
        {
            _bIsVisible = true;
        }
    }
}
