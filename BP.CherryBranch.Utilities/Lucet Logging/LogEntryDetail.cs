using System;
using System.Runtime.Serialization;

namespace Lucet.CherryBranch.Utilities.Logging
{
    /// <summary>
	/// LogEntryDetail Class
	/// </summary>
	[Serializable]
    [DataContract]
    public class LogEntryDetail
    {
        #region Constants
        /// <summary>
        /// A constant containing the numeric value used to identify an unspecified detail id
        /// </summary>
        public const long UnspecifiedLogDetailId = -1;
        #endregion //Constants

        #region Public Properties
        /// <summary>
        /// Log Detail Id
        /// </summary>
        [DataMember]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Log Detail Name
        /// </summary>
        [DataMember]
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Log Detail Value
        /// </summary>
        [DataMember]
        public string Value
        {
            get;
            set;
        }
        #endregion //Public Properties

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LogEntryDetail() : this(UnspecifiedLogDetailId, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        public LogEntryDetail(string code, string value) : this(UnspecifiedLogDetailId, code, value)
        {
        }

        /// <summary>
        /// Full Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        public LogEntryDetail(long id, string code, string value)
        {
            Id = id;
            Code = code;
            Value = value;
        }
        #endregion //Constructors
    }
}
