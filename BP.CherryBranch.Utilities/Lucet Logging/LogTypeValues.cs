using System;

namespace Lucet.CherryBranch.Utilities.Logging
{
    /// <summary>
	/// Constants for log types
	/// </summary>
	public class LogTypeValues
    {
        #region Public Fields

        /// <summary>
        /// None
        /// </summary>
        /// <remarks>
        /// LogLevel.Trace = 0
        /// </remarks>
        public const string Trace = "Trace";

        /// <summary>
        /// Debug
        /// </summary>
        /// <remarks>
        /// LogLevel.Debug = 1
        /// </remarks>
        public const string Debug = "Debug";

        /// <summary>
        /// Information
        /// </summary>
        /// <remarks>
        /// LogLevel.Information = 2
        /// </remarks>
		public const string Information = "Information";

        /// <summary>
        /// Warning
        /// </summary>
        /// <remarks>
        /// LogLevel.Warning = 3
        /// </remarks>
        public const string Warning = "Warning";

        /// <summary>
        /// Error
        /// </summary>
        /// <remarks>
        /// LogLevel.Error = 4
        /// </remarks>
        public const string Error = "Error";

        /// <summary>
        /// Critical
        /// </summary>
        /// <remarks>
        /// LogLevel.Critical = 5
        /// </remarks>
        public const string Critical = "Critical";

        /// <summary>
        /// None
        /// </summary>
        /// <remarks>
        /// LogLevel.None = 6
        /// </remarks>
        public const string None = "None";

        #endregion //Public Fields
    }
}
