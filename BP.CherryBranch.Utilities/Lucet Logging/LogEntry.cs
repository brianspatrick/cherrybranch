using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lucet.CherryBranch.Utilities.Logging
{
    /// <summary>
	/// LogEntry Class
	/// </summary>
	[Serializable]
    [DataContract]
    public class LogEntry
    {
        #region Constants
        /// <summary>
        /// A constant containing the numeric value used to identify an unspecified log id
        /// </summary>
        public const long UnspecifiedLogId = -1;
        #endregion //Constants

        #region Public Properties
        /// <summary>
        /// Log Id
        /// </summary>
        [DataMember]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Log Type
        /// </summary>
        [DataMember]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Log Date
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// Log Description
        /// </summary>
        [DataMember]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Log Details
        /// </summary>
        [DataMember]
        public List<LogEntryDetail> LogDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Are there Log Details
        /// </summary>
        [DataMember]
        public bool HasLogDetails
        {
            get
            {
                return ((LogDetails != null) && (LogDetails.Count > 0));
            }

            set
            {
            }
        }
        #endregion //Public Properties

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LogEntry() : this(UnspecifiedLogId, String.Empty, DateTime.MinValue, String.Empty, new List<LogEntryDetail>())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <param name="date"></param>
        public LogEntry(string type, string description, DateTime date) : this(UnspecifiedLogId, type, date, description, new List<LogEntryDetail>())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <param name="date"></param>
        /// <param name="logDetails"></param>
        public LogEntry(string type, string description, DateTime date, List<LogEntryDetail> logDetails) : this(UnspecifiedLogId, type, date, description, logDetails)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="logDetails"></param>
        public LogEntry(long id, string type, DateTime date, List<LogEntryDetail> logDetails) : this(id, type, date, String.Empty, logDetails)
        {
        }

        /// <summary>
        /// Full Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="description"></param>
        /// <param name="logDetails"></param>
        public LogEntry(long id, string type, DateTime date, string description, List<LogEntryDetail> logDetails)
        {
            Id = id;
            Type = type;
            Date = date;
            Description = description;
            LogDetails = logDetails;
        }
        #endregion //Constructors
    }
}
