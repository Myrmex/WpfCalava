using System;
using System.ComponentModel.Composition;
using WpfCalava.Assets;

namespace WpfCalava.ViewModels
{
    /// <summary>
    /// An example document represented by a simple note text.
    /// </summary>
    [Export(typeof(NoteViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class NoteViewModel : DocumentBase
    {
        private string _sText;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return _sText; }
            set
            {
                if (value == _sText) return;
                _sText = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        /// <summary>
        /// Determines whether the item being edited is dirty.
        /// </summary>
        /// <returns>true if dirty</returns>
        protected override bool IsDirty()
        {
            // To simplify this example, we do not implement save and the like;
            // just type "saved" in its text to let this note be considered as not dirty.
            return ((!String.IsNullOrEmpty(_sText)) && (!_sText.Contains("saved")));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteViewModel"/> class.
        /// </summary>
        public NoteViewModel()
        {
            DisplayName = StringResources.Note;
        }
    }
}
