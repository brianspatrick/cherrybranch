using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Lucet.CherryBranch.Utilities.Common;

namespace Lucet.CherryBranch.Utilities.Logging
{
    /// <summary>
    /// LucetLogger Class
    /// </summary>
    public class LucetLogger
    {
        #region Constants
        private const string ExternalServicesKey = "LucetApi";
        private const string ExternalServicesConfigPath = $"{ExternalServicesKey}";
        private const string ServiceUrlConfigPath = $"{ExternalServicesConfigPath}:BaseAddress";
        private const string ClientIdConfigPath = $"{ExternalServicesConfigPath}:{ApiAuthorizationValues.ClientIdValue}";
        private const string ApiKeyConfigPath = $"{ExternalServicesConfigPath}:{ApiAuthorizationValues.ApiKeyValue}";

        private const string DefaultLogType = LogTypeValues.None;
        #endregion //Constants

        #region Private Fields

        //Configuration
        private static IConfiguration _configuration = null;

        //Logging service Url
        private static string _serviceUrl = String.Empty;

        private static string _clientId = String.Empty;
        private static string _apiKey = String.Empty;
        #endregion //Private Fields

        #region Public Properties
        /// <summary>
        /// Configuration
        /// </summary>
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();
                }

                return _configuration;
            }
        }
        #endregion //Public Properties

        #region Public Methods

        #region Log
        /// <summary>
        /// Log
        /// </summary>
        /// <param name="description"></param>
        public static void Log(string description)
        {
            try
            {
                Log(description, DefaultLogType, null);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="description"></param>
        /// <param name="details"></param>
        public static void Log(string description, Dictionary<string, string> details)
        {
            try
            {
                Log(description, DefaultLogType, details);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="description"></param>
        /// <param name="logType"></param>
        /// <param name="details"></param>
        public static void Log(string description, string logType, Dictionary<string, string> details)
        {
            try
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Description = description;
                logEntry.Type = logType;
                logEntry.Date = DateTime.Now.ToTimeZone("Central Standard Time");

                if ((details != null) && (details.Count > 0))
                {
                    foreach (KeyValuePair<string, string> pair in details)
                    {
                        logEntry.LogDetails.Add(new LogEntryDetail(pair.Key, pair.Value));
                    }
                }

                Log(logEntry);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="exception"></param>
        public static void Log(Exception exception)
        {
            try
            {
                Log(exception.Message, exception);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="description"></param>
        /// <param name="exception"></param>
        public static void Log(string description, Exception exception)
        {
            try
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Description = description;
                logEntry.Type = LogTypeValues.Error;
                logEntry.Date = DateTime.Now.ToTimeZone("Central Standard Time");

                logEntry.LogDetails.Add(new LogEntryDetail("error", HelperFunctions.BuildErrorDetails(exception, "\n")));

                Log(logEntry);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="description"></param>
        /// <param name="details"></param>
        /// <param name="exception"></param>
        public static void Log(string description, Dictionary<string, string> details, Exception exception)
        {
            try
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Description = description;
                logEntry.Type = LogTypeValues.Error;
                logEntry.Date = DateTime.Now.ToTimeZone("Central Standard Time");

                logEntry.LogDetails.Add(new LogEntryDetail("error", HelperFunctions.BuildErrorDetails(exception, "\n")));

                if ((details != null) && (details.Count > 0))
                {
                    foreach (KeyValuePair<string, string> pair in details)
                    {
                        logEntry.LogDetails.Add(new LogEntryDetail(pair.Key, pair.Value));
                    }
                }

                Log(logEntry);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="logEntry"></param>
        public static void Log(LogEntry logEntry)
        {
            string serviceName = "logging";
            string restMethod = "log";
            StringBuilder errorMessage = new StringBuilder();

            try
            {
                //get the logging endpoint url from the configuration
                if (String.IsNullOrEmpty(_serviceUrl))
                {
                    _serviceUrl = ((!String.IsNullOrEmpty(Configuration[ServiceUrlConfigPath])) ? Configuration[ServiceUrlConfigPath] : String.Empty);
                }

                if (String.IsNullOrEmpty(_clientId))
                {
                    _clientId = ((!String.IsNullOrEmpty(Configuration[ClientIdConfigPath])) ? Configuration[ClientIdConfigPath] : String.Empty);
                }
                
                if (String.IsNullOrEmpty(_apiKey))
                {
                    _apiKey = ((!String.IsNullOrEmpty(Configuration[ApiKeyConfigPath])) ? Configuration[ApiKeyConfigPath] : String.Empty);
                }          

                if (!String.IsNullOrEmpty(_serviceUrl))
                {
                    if (logEntry != null)
                    {
                        //send the LogEntry to the logging service endpoint url
                        string responseContent = String.Empty;

                        RestClientOptions options = new RestClientOptions(_serviceUrl);

                        JsonSerializerSettings defaultSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            DefaultValueHandling = DefaultValueHandling.Include,
                            TypeNameHandling = TypeNameHandling.None,
                            NullValueHandling = NullValueHandling.Ignore,
                            Formatting = Formatting.None,
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                        };

                        RestClient client = new RestClient(
                            options,
                            configureSerialization: s => s.UseNewtonsoftJson(defaultSettings)
                        );

                        RestRequest request = new RestRequest($"{serviceName}/{restMethod}", Method.Post);

                        //add the authorization headers
                        request.AddHeader(ApiAuthorizationValues.ClientIdValue, _clientId);
                        request.AddHeader(ApiAuthorizationValues.ApiKeyValue, _apiKey);

                        request.AddJsonBody(logEntry);

                        RestResponse response = client.Execute(request);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            responseContent = response.Content;
                        }
                        else
                        {
                            responseContent = response.Content;
                            errorMessage.Append("Error retrieving response");
                            errorMessage.AppendFormat("{0}Service URL: {1}/{2}", System.Environment.NewLine, _serviceUrl, restMethod);
                            errorMessage.AppendFormat("{0}Status Code: {1} ({2})", System.Environment.NewLine, (int)Enum.Parse(typeof(HttpStatusCode), response.StatusCode.ToString()), response.StatusCode);
                            errorMessage.AppendFormat("{0}Response Content: {1}", System.Environment.NewLine, response.Content);
     
                            if (response.ErrorException != null)
                            {
                                errorMessage.AppendFormat("{0}Error:{0}{1}", System.Environment.NewLine, HelperFunctions.BuildErrorDetails(response.ErrorException, "\n"));
                            }
                        }

                        if (errorMessage.Length > 0)
                        {
                            System.Diagnostics.Trace.WriteLine($"Unable to write log entry to the service url due to the following error:{Environment.NewLine}{errorMessage.ToString()}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("The log entry is NULL");
                    }
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("The service url is NULL or EMPTY");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }
        #endregion //Log

        #endregion //Public Methods
    }
}
