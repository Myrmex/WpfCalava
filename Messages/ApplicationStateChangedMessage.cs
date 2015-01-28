using System;
using System.Globalization;

namespace WpfCalava.Messages
{
    /// <summary>
    /// Message fired whenever the application state changed.
    /// </summary>
    public class ApplicationStateChangedMessage
    {
        public string PropertyName { get; private set; }
        public object NewValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStateChangedMessage"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property which changed.</param>
        /// <param name="newValue">The new property value.</param>
        public ApplicationStateChangedMessage(string propertyName, object newValue)
        {
            PropertyName = propertyName;
            NewValue = newValue;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}={1}",
                PropertyName, NewValue);
        }
    }
}
