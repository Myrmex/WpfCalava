using System;
using System.Windows;
using Caliburn.Micro;
using WpfCalava.Actions;
using WpfCalava.Assets;

namespace WpfCalava.ViewModels
{
    /// <summary>
    /// Base class for document viewmodels.
    /// </summary>
    public abstract class DocumentBase : Screen
    {
        private bool _bIsForcedClosing; //@@hack
        private string _sId;

        /// <summary>
        /// Gets or sets the document content identifier. This might be a file path,
        /// a database PK, etc.
        /// </summary>
        public string Id
        {
            get { return _sId; }
            set
            {
                if (value == _sId) return;
                _sId = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        /// <summary>
        /// Determines whether the item being edited is dirty.
        /// </summary>
        /// <returns>true if dirty</returns>
        protected abstract bool IsDirty();

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result 
        /// of the close check.</param>
        public override void CanClose(Action<bool> callback)
        {
            // if forcing (@@hack) or not dirty it's OK to close
            if ((_bIsForcedClosing) || (!IsDirty()))
            {
                callback(true);
                return;
            } //eif

            // else prompt user
            MessageBoxAction prompt = new MessageBoxAction
            {
                Caption = App.APP_NAME,
                Text = LocalizedStrings.Format(StringResources.DiscardDocumentChangesPrompt,
                    DisplayName),
                Button = MessageBoxButton.YesNo,
                Image = MessageBoxImage.Question
            };
            prompt.Completed += (sender, e) =>
            {
                callback(prompt.Result == MessageBoxResult.Yes);
            };
            prompt.Execute(null);
        }

        //@@hack: workaround for main view code behind
        /// <summary>
        /// Determines whether this instance can close. This is meant to be called by the view
        /// code behind when testing if a document can close (OnDocumentClosing).
        /// </summary>
        /// <returns>true if can close</returns>
        public bool CanClose()
        {
            if (!IsDirty()) return true;

            MessageBoxAction prompt = new MessageBoxAction
            {
                Caption = App.APP_NAME,
                Text = LocalizedStrings.Format(StringResources.DiscardDocumentChangesPrompt,
                    DisplayName),
                Button = MessageBoxButton.YesNo,
                Image = MessageBoxImage.Question
            };

            bool bResult = true;
            prompt.Completed += (sender, e) =>
            {
                bResult = prompt.Result == MessageBoxResult.Yes;
            };
            prompt.Execute(null);
            return bResult;
        }

        //@@hack: workaround for main view code behind
        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <remarks>This forces closing even when the document is dirty. This is meant
        /// to be called by the view code behind once the document has been closed
        /// (OnDocumentClosed), as the dirty check at that point has already been passed.
        /// Thus, we could even remove the dirty check code in the CanClose override,
        /// but this is a hack and I hope we'll be able to defer this check back to the
        /// VM without all this, so I kept that code there.</remarks>
        public void Close()
        {
            _bIsForcedClosing = true;
            try
            {
                TryClose();
            }
            finally
            {
                _bIsForcedClosing = false;
            }
        }
    }
}
