using System.Windows;
using System.Windows.Controls;

namespace WpfCalava.Views
{
    // autobinder:
    // http://stackoverflow.com/questions/14546583/avalondock-2-with-caliburn-micro
    // general:
    // https://caliburnmicro.codeplex.com/discussions/430994

    public class AutobinderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return Template;
        }
    }
}
