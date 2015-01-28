using System;
using System.Diagnostics;
using System.Globalization;
using Caliburn.Micro;

namespace WpfCalava
{
    /// <summary>
    /// Debug logger for Caliburn Micro.
    /// </summary>
    /// <remarks>See http://buksbaum.us/2010/08/08/how-to-do-logging-with-caliburn-micro/ </remarks>
    public sealed class DebugLogger : ILog
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DebugLogger(Type type)
        {
            _type = type;
        }

        private static string CreateLogMessage(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "[{0}] {1}",
                DateTime.Now.ToString("o"),
                String.Format(CultureInfo.InvariantCulture, format, args));
        }

        #region ILog Members
        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            Debug.WriteLine(CreateLogMessage(exception.ToString()), "ERROR");
        }

        /// <summary>
        /// Logs the message as info.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public void Info(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "INFO");
        }

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args), "WARN");
        }
        #endregion
    }
}
