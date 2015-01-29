using System;
using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using WpfCalava.Assets;

namespace WpfCalava.ViewModels
{
    /// <summary>
    /// A dummy tool pane just to have something to play with.
    /// </summary>
    [Export(typeof(WordCounterViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class WordCounterViewModel : ToolBase
    {
        private string _sText;
        private int _nCount;

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

        public int Count
        {
            get { return _nCount; }
            set
            {
                if (value == _nCount) return;
                _nCount = value;
                NotifyOfPropertyChange(() => Count);
            }
        }

        public void CountWords()
        {
            if (String.IsNullOrWhiteSpace(_sText))
            {
                Count = 0;
                return;
            }

            Count = _sText.Split().Length;
        }

        //[ImportingConstructor]
        public WordCounterViewModel()
        {
            DisplayName = StringResources.WordCounter;
            IconSource = new BitmapImage(new Uri("pack://siteoforigin:,,,/Assets/Images/Notes.png"));
        }
    }
}
