using System;
using System.Windows;
using Caliburn.Micro;

namespace WpfCalava.Actions
{
    /// <summary>
    /// Generic message box prompt.
    /// </summary>
    public sealed class MessageBoxAction : IResult
    {
        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Button(s).
        /// </summary>
        public MessageBoxButton Button { get; set; }

        /// <summary>
        /// Image.
        /// </summary>
        public MessageBoxImage Image { get; set; }

        /// <summary>
        /// Result.
        /// </summary>
        public MessageBoxResult Result { get; private set; }

        /// <summary>
        /// Executes the result using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(CoroutineExecutionContext context)
        {
            Result = MessageBox.Show(Text, Caption, Button, Image);
            if (Completed != null)
                Completed(this, new ResultCompletionEventArgs());
        }

        /// <summary>
        /// Occurs when execution has completed.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed;
    }
}
