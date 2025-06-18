using System;

namespace Lucet.CherryBranch.Utilities.Common
{
    /// <summary>
    /// DateTimeExtensionMethods Class
    /// </summary>
    public static class DateTimeExtensionMethods
    {
        #region ToTimeZone
        /// <summary>
        /// Convert a DateTime to the specified TimeZone
        /// </summary>
        public static DateTime ToTimeZone(this DateTime dateTime, string timeZoneId)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
        }
        #endregion //ToTimeZone
    }
}
