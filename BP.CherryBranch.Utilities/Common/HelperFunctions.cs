using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Lucet.CherryBranch.Utilities.Common
{
    /// <summary>
    /// NameOrder Enumeration
    /// </summary>
    [Serializable]
    public enum NameOrder
    {
        LastNameFirst = 1,
        FirstNameFirst = 2
    }

    /// <summary>
    /// HelperFunctions
    /// </summary>
    public class HelperFunctions
    {
        #region Public Methods

        #region FormatName
        /// <summary>
        /// Formats a name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        /// <param name="suffix"></param>
        /// <param name="degree"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string FormatName(string firstName, string lastName, string middleName, string suffix, string degree, NameOrder order)
        {
            StringBuilder sb = new StringBuilder();

            if (order == NameOrder.LastNameFirst)   //last suffix, first middle, degree
            {
                if ((!String.IsNullOrEmpty(lastName)) && (lastName.Trim().Length > 0))
                    sb.Append(lastName.Trim());

                if (((!String.IsNullOrEmpty(suffix)) && (suffix.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(" {0}", suffix.Trim()) : suffix.Trim());

                if (((!String.IsNullOrEmpty(firstName)) && (firstName.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(", {0}", firstName.Trim()) : firstName.Trim());

                if (((!String.IsNullOrEmpty(middleName)) && (middleName.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(" {0}", middleName.Trim()) : middleName.Trim());

                if (((!String.IsNullOrEmpty(degree)) && (degree.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(", {0}", degree.Trim()) : degree.Trim());
            }
            else if (order == NameOrder.FirstNameFirst) //first middle last, suffix, degree
            {
                if (((!String.IsNullOrEmpty(firstName)) && (firstName.Trim().Length > 0)))
                    sb.Append(firstName.Trim());

                if (((!String.IsNullOrEmpty(middleName)) && (middleName.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(" {0}", middleName.Trim()) : middleName.Trim());

                if (((!String.IsNullOrEmpty(lastName)) && (lastName.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(" {0}", lastName.Trim()) : lastName.Trim());

                if (((!String.IsNullOrEmpty(suffix)) && (suffix.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(", {0}", suffix.Trim()) : suffix.Trim());

                if (((!String.IsNullOrEmpty(degree)) && (degree.Trim().Length > 0)))
                    sb.Append((sb.Length > 0) ? String.Format(", {0}", degree.Trim()) : degree.Trim());
            }

            return sb.ToString();
        }
        #endregion //FormatName

        #region FormatAddress
        /// <summary>
        /// Formats an address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="zip4"></param>
        /// <returns></returns>
        public static string FormatAddress(string address, string address2, string city, string state, string zip, string zip4)
        {
            //address, address2
            //city, state zip-zip4
            StringBuilder sb = new StringBuilder();

            if ((!String.IsNullOrEmpty(address)) && (address.Trim().Length > 0))
                sb.Append(address.Trim());

            if ((!String.IsNullOrEmpty(address2)) && (address2.Trim().Length > 0))
                sb.Append(((sb.Length > 0) ? String.Format(", {0}", address2.Trim()) : address2.Trim()));

            if ((!String.IsNullOrEmpty(city)) && (city.Trim().Length > 0))
                sb.Append(((sb.Length > 0) ? String.Format("<br />{0}", city.Trim()) : city.Trim()));

            if ((!String.IsNullOrEmpty(state)) && (state.Trim().Length > 0))
                sb.Append(((sb.Length > 0) ? String.Format(", {0}", state.Trim()) : state.Trim()));

            if ((!String.IsNullOrEmpty(zip)) && (zip.Trim().Length > 0))
                sb.Append(((sb.Length > 0) ? String.Format(" {0}", zip.Trim()) : zip.Trim()));

            if ((!String.IsNullOrEmpty(zip4)) && (zip4.Trim().Length > 0))
                sb.Append(((sb.Length > 0) ? String.Format("-{0}", zip4.Trim()) : zip4.Trim()));

            return sb.ToString();
        }
        #endregion //FormatAddress

        #region FormatPhone
        /// <summary>
        /// Format a ten-digit phone number
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string FormatPhone(string phone)
        {
            if ((phone != null) && (phone.Length == 10))
            {
                return System.Text.RegularExpressions.Regex.Replace(phone, @"(?<areacode>\d{3})(?<exchange>\d{3})(?<ext>\d{4})", "${areacode}-${exchange}-${ext}");
            }

            return string.Empty;
        }
        #endregion //FormatPhone

        #region FormatPhoneWithParens
        /// <summary>
        /// Format a ten-digit phone number with parentheses around the area code
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string FormatPhoneWithParens(string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                if (phone.Contains('@')) return phone;

                if ((phone != null) && (phone.Length == 10))
                {
                    return System.Text.RegularExpressions.Regex.Replace(phone, @"(?<areacode>\d{3})(?<exchange>\d{3})(?<ext>\d{4})", "(${areacode}) ${exchange}-${ext}");
                }
            }

            return "Not Available";
        }
        #endregion //FormatPhoneWithParens

        #region BuildCacheKey
        /// <summary>
        /// Helper for building a cache key in a standard format
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public static string BuildCacheKey(string[] names)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string name in names)
            {
                if ((name != null) && (name.Length > 0))
                {
                    sb.AppendFormat("{0}_", name);
                }
            }

            return sb.ToString().TrimEnd(new char[] { '_' });
        }
        #endregion //BuildCacheKey

        #region IEnumerableToList
        /// <summary>
        /// Helper for converting an IEnumerable to a Generic List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> IEnumerableToList<T>(IEnumerable<T> list)
        {
            List<T> returnList = new List<T>();

            foreach (T item in list)
            {
                returnList.Add(item);
            }

            return returnList;
        }
        #endregion //IEnumerableToList

        #region StringToEnum
        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }
        #endregion //StringToEnum

        #region StripUnwantedCharacters
        /// <summary>
        /// Strip Unwanted Characters
        /// </summary>
        /// <param name="stringToSearch"></param>
        /// <param name="charactersToReplace"></param>
        /// <returns></returns>
        public static string StripUnwantedCharacters(string stringToStrip, string[] unwantedCharacters)
        {
            if (!String.IsNullOrEmpty(stringToStrip))
            {
                foreach (string unwantedCharacter in unwantedCharacters)
                {
                    stringToStrip = stringToStrip.Trim().Replace(unwantedCharacter, String.Empty);
                }
            }

            return stringToStrip;
        }
        #endregion //StripUnwantedCharacters

        #region GetLocalIPAddress
        /// <summary>
        /// Get the IPAddress of the current machine
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            string ipAddress = "Unknown";

            try
            {
                IPAddress machineIPAddress = GetIPAddress();

                if (machineIPAddress != null)
                {
                    ipAddress = machineIPAddress.ToString();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return ipAddress;
        }
        #endregion //GetLocalIPAddress

        #region GetLocalIPAddressString
        /// <summary>
        /// Get the IPAddress of the current machine
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddressString()
        {
            string ipAddress = "Unknown";

            try
            {
                IPAddress machineIPAddress = GetIPAddress();

                if (machineIPAddress != null)
                {
                    ipAddress = machineIPAddress.ToString();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return ipAddress;
        }
        #endregion //GetLocalIPAddressString

        #region GetIPAddress
        /// <summary>
        /// Get the IPAddress of the current machine
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetIPAddress()
        {
            IPAddress ipAddress = null;

            try
            {
                string machineName = GetMachineName();

                List<IPAddress> list = GetIPAddresses(machineName);

                if ((list != null) && (list.Count > 0))
                {
                    foreach (IPAddress item in list)
                    {
                        if (item.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return ipAddress;
        }
        #endregion //GetIPAddress

        #region GetIPAddresses
        /// <summary>
        /// Get the IPAddresses of the specified machine
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public static List<IPAddress> GetIPAddresses(string machineName)
        {
            List<IPAddress> ipAddresses = new List<IPAddress>();

            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(machineName);

                if ((hostEntry != null) && (hostEntry.AddressList != null) && (hostEntry.AddressList.Length > 0))
                {
                    foreach (IPAddress ipAddress in hostEntry.AddressList)
                    {
                        ipAddresses.Add(ipAddress);
                    }
                }
            }
            catch (SocketException se)
            {
                System.Diagnostics.Trace.WriteLine(se.ToString());
                //eat it!
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return ipAddresses;
        }
        #endregion //GetIPAddresses

        #region GetUserHostAddress
        /// <summary>
        /// GetUserHostAddress
        /// </summary>
        /// <param name="userHostAddress"></param>
        /// <returns></returns>
        public static string GetUserHostAddress(string userHostAddress)
        {
            string ipAddress = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(userHostAddress))
                {
                    foreach (IPAddress ip in Dns.GetHostAddresses(userHostAddress))
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.ToString();
                            break;
                        }
                    }
                }

                if (String.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = GetLocalIPAddress();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return ipAddress;
        }
        #endregion //GetUserHostAddress

        #region IsLocalIPAddress
        /// <summary>
        /// Is Local IPAddress
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsLocalIPAddress(string host)
        {
            try
            {
                // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);

                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP))
                    {
                        return true;
                    }

                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return false;
        }
        #endregion //IsLocalIPAddress

        #region GetMachineName
        /// <summary>
        /// Get the name of the current machine
        /// </summary>
        /// <returns></returns>
        public static string GetMachineName()
        {
            string machineName = String.Empty;

            try
            {
                string machineNameFromDns = String.Empty;

                try
                {
                    machineNameFromDns = Dns.GetHostName();
                }
                catch (SocketException se)
                {
                    System.Diagnostics.Trace.WriteLine(se.ToString());
                    //eat it!
                }

                if (!String.IsNullOrEmpty(machineNameFromDns))
                {
                    machineName = machineNameFromDns;
                }
                else
                {
                    string machineNameFromEnvironment = String.Empty;

                    try
                    {
                        machineNameFromEnvironment = Environment.MachineName;
                    }
                    catch (InvalidOperationException ioe)
                    {
                        System.Diagnostics.Trace.WriteLine(ioe.ToString());
                        //eat it!
                    }

                    if (!String.IsNullOrEmpty(machineNameFromEnvironment))
                    {
                        machineName = machineNameFromEnvironment;
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return machineName;
        }
        #endregion //GetMachineName

        #region GetLocalMachineName
        /// <summary>
        /// Get the name of the current machine
        /// </summary>
        /// <returns></returns>
        public static string GetLocalMachineName()
        {
            string machineName = String.Empty;

            try
            {
                machineName = GetMachineName();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                throw;
            }

            return ((!String.IsNullOrEmpty(machineName)) ? machineName : "Unknown");
        }
        #endregion //GetLocalMachineName

        #region FormatStackTrace
        /// <summary>
        /// Format Stack Trace
        /// </summary>
        /// <param name="stackTrace"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public static string FormatStackTrace(string stackTrace, string newLine)
        {
            StringBuilder stackTraceFormatted = new StringBuilder();
            string[] stackTraceSplit = stackTrace.Split(new string[] { " at " }, StringSplitOptions.RemoveEmptyEntries);

            if ((stackTraceSplit != null) && (stackTraceSplit.Length > 0))
            {
                foreach (string stackTraceItem in stackTraceSplit)
                {
                    if ((!String.IsNullOrEmpty(stackTraceItem)) && (stackTraceItem.Length > 2) && (0 != String.Compare(stackTraceItem, "  ", true)))
                    {
                        stackTraceFormatted.AppendFormat("{0}at {1}", ((stackTraceFormatted.Length > 0) ? newLine : String.Empty), stackTraceItem.Replace(" in ", String.Format("{0}in ", newLine)));
                    }
                }
            }
            else
            {
                stackTraceFormatted.Append(stackTrace);
            }

            return stackTraceFormatted.ToString();
        }
        #endregion //FormatStackTrace

        #region BuildErrorDetails
        /// <summary>
        /// Build Error Details
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public static string BuildErrorDetails(Exception exception, string newLine)
        {
            StringBuilder errorDetails = new StringBuilder();

            if (exception != null)
            {
                Exception exceptionTemp = exception;
                int innerExceptionCount = 0;

                //build a string of the original exception and all of the inner exceptions so nothing is lost
                while (exceptionTemp != null)
                {
                    if (innerExceptionCount == 0)
                    {
                        errorDetails.Append("Exception [Top Level]:");
                    }
                    else if (exceptionTemp.InnerException == null)
                    {
                        errorDetails.AppendFormat("{0}{0}Exception [Root]:", newLine);
                    }
                    else
                    {
                        errorDetails.AppendFormat("{0}{0}Exception [Inner][{1}]:", newLine, innerExceptionCount);
                    }

                    errorDetails.AppendFormat("{0}Message: {1}", newLine, exceptionTemp.Message.Replace(Environment.NewLine, newLine));
                    errorDetails.AppendFormat("{0}Source: {1}", newLine, exceptionTemp.Source);
                    errorDetails.AppendFormat("{0}TargetSite: {1}", newLine, ((exceptionTemp.TargetSite != null) ? exceptionTemp.TargetSite.Name : "null"));
                    errorDetails.AppendFormat("{0}StackTrace: {1}", newLine, ((!String.IsNullOrEmpty(exceptionTemp.StackTrace)) ? String.Format("{0}{1}", newLine, HelperFunctions.FormatStackTrace(exceptionTemp.StackTrace, newLine)) : "null"));

                    exceptionTemp = exceptionTemp.InnerException;

                    innerExceptionCount++;
                }
            }

            return errorDetails.ToString();
        }
        #endregion //BuildErrorDetails

        #region BuildErrorMessages
        /// <summary>
        /// Build Error Messages
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        public static List<string> BuildErrorMessages(Exception exception, string newLine)
        {
            List<string> errorMessages = new List<string>();

            if (exception != null)
            {
                Exception exceptionTemp = exception;
                int innerExceptionCount = 0;

                //build a list of the exceptions messages
                while (exceptionTemp != null)
                {
                    string levelText = String.Empty;

                    if (innerExceptionCount == 0)
                    {
                        levelText = "[Top Level]";
                    }
                    else if (exceptionTemp.InnerException == null)
                    {
                        levelText = "[Root]";
                    }
                    else
                    {
                        levelText = $"[Inner][{innerExceptionCount}]";
                    }

                    errorMessages.Add($"Exception {levelText}: {exceptionTemp.Message.Replace(Environment.NewLine, newLine)}");

                    exceptionTemp = exceptionTemp.InnerException;

                    innerExceptionCount++;
                }
            }

            return errorMessages;
        }
        #endregion //BuildErrorMessages

        #region FormatErrorDetails
        /// <summary>
        /// Format Error Details
        /// </summary>
        /// <returns></returns>
        public static string FormatErrorDetails(string details)
        {
            string formattedDetails = String.Empty;

            if (!String.IsNullOrEmpty(details))
            {
                formattedDetails = details.Replace("\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            }

            return formattedDetails;
        }
        #endregion //FormatErrorDetails

        #region DateTime Methods

        /// <summary>
        /// Returns the current Central Standard Date/Time.
        /// </summary>
        public static DateTime CentralTimeStamp()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        #endregion DateTime Methods

        #endregion //Public Methods
    }
}
