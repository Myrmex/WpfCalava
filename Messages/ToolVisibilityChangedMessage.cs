using System;
using WpfCalava.ViewModels;

namespace WpfCalava.Messages
{
    //@@hack
    public sealed class ToolVisibilityChangedMessage
    {
        public ToolBase Sender { get; private set; }

        public ToolVisibilityChangedMessage(ToolBase tool)
        {
            if (tool == null) throw new ArgumentNullException("tool");
            Sender = tool;
        }
    }
}
