using Microsoft.ApplicationInsights;
using SD = System.Diagnostics;
using Lucet.CherryBranch.Utilities.Logging;
using Lucet.CherryBranch.Utilities.Common;
using Microsoft.Extensions.Configuration;
using System;

namespace Lucet.CherryBranch.Utilities
{
    public class Logger
    {
        #region Private Fields

        //Configuration
        private static IConfiguration _configuration = null;

        //Application Insights Instrumentation Key
        private static string _instrumentationKey = String.Empty;

        //Telemetry Client
        private static TelemetryClient? _telemetryClient;

        #endregion //Private Fields

        #region Private Properties
        /// <summary>
        /// Configuration
        /// </summary>
        private static IConfiguration Configuration
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

        /// <summary>
        /// Telemetry Client
        /// </summary>
        private static TelemetryClient TelemetryClient
        {
            get
            {
                if (_telemetryClient == null)
                {
                    _telemetryClient = new TelemetryClient() { InstrumentationKey = _instrumentationKey };
                }

                return _telemetryClient;
            }
        }

        /// <summary>
        /// Instrumentation Key
        /// </summary>
        private static string InstrumentationKey
        {
            get
            {
                if (String.IsNullOrEmpty(_instrumentationKey))
                {
                    _instrumentationKey = Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
                }

                return _instrumentationKey;
            }
        }
        #endregion //Private Properties

        #region Public Methods

        #region Trace

        /// <summary>
        /// StampTrace Logging
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleTrace(string message, ConsoleColor textColor = ConsoleColor.Green)
        {
            try
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = textColor;
                Console.WriteLine($"[{DateTime.Now.ToTimeZone("Central Standard Time"):yyyy-MM-dd hh:mm:ss.fff tt}] ---------------------------------------------------------------------");
                Console.WriteLine($"[{DateTime.Now.ToTimeZone("Central Standard Time"):yyyy-MM-dd hh:mm:ss.fff tt}]  {message}");
                Console.WriteLine($"[{DateTime.Now.ToTimeZone("Central Standard Time"):yyyy-MM-dd hh:mm:ss.fff tt}] ---------------------------------------------------------------------");
                Console.ForegroundColor = currentColor;
            }
            catch { }
        }

        /// <summary>
        /// StampTrace Logging
        /// </summary>
        /// <param name="message"></param>
        public static void StampTrace(string message)
        {
            try
            {
                //on premise log
                LucetTrace($"[{DateTime.Now.ToTimeZone("Central Standard Time"):yyyy-MM-dd hh:mm:ss.fff tt}] {message}");

                //application insights log
                TrackTrace($"[{DateTime.Now.ToTimeZone("Central Standard Time"):yyyy-MM-dd hh:mm:ss.fff tt}] {message}");
            }
            catch { }
        }

        /// <summary>
        /// Trace Logging
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            try
            {
                //on premise log
                LucetTrace(message);

                //application insights log
                TrackTrace(message);
            }
            catch { }
        }

        /// <summary>
        /// Trace Logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Trace(string message, Exception exception)
        {
            try
            {
                //on premise log
                LucetTrace(message, exception);

                //application insights log
                TrackException(exception, $"{message} -> {exception.Message} | {exception.StackTrace}");
            }
            catch { }
        }

        /// <summary>
        /// Trace Logging
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="messages"></param>
        public static void Trace(string eventName, Dictionary<string, string> messages)
        {
            try
            {
                //on premise log
                LucetTrace(eventName, messages);

                //application insights log
                Task.Run(() =>
                {
                    TelemetryClient.TrackEvent(eventName, messages);
                });

                foreach (var property in messages.ToList())
                {
                    Console.WriteLine($"Event: [{eventName}] | {property.Key}: [{property.Value}]");
                }
            }
            catch { }
        }
        #endregion //Trace

        #region Exception
        /// <summary>
        /// Exception Logging
        /// </summary>
        /// <param name="exception"></param>
        public static void Exception(Exception exception)
        {
            try
            {
                //on premise log
                LucetTrace(exception);

                //application insights log
                TrackException(exception, $"{exception.Message} | {exception.StackTrace}");

                Task.Run(() =>
                {
                    TelemetryClient.TrackException(exception);
                });
            }
            catch { }
        }

        /// <summary>
        /// Exception Logging
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="messages"></param>
        /// <param name="exception"></param>
        public static void Exception(string eventName, Dictionary<string, string> messages, Exception exception)
        {
            try
            {
                //on premise log
                LucetTrace(eventName, messages, exception);

                // Application Insights Logging
                messages.Add("event name", eventName);

                Task.Run(() =>
                {
                    TelemetryClient.TrackEvent(eventName, messages);
                    TelemetryClient.TrackException(exception, messages);
                });

                foreach (var property in messages.ToList())
                {
                    Console.WriteLine($"Event: [{eventName}] | {property.Key}: [{property.Value}]");
                }
            }
            catch { }
        }
        #endregion //Exception

        #endregion //Public Methods

        #region Private Methods

        #region Trace
        /// <summary>
        /// Trace Logging
        /// </summary>
        /// <remarks>
        /// This Logs to Application Insights
        /// </remarks>
        /// <param name="message"></param>
        private static void TrackTrace(string message)
        {
            try
            {
                Task.Run(() =>
                {
                    Console.WriteLine(message);

                    TelemetryClient.TrackTrace(message);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{message} | Log Failure: {e.Message} | {e.Message}");
            }
        }

        /// <summary>
        /// Exception Logging
        /// </summary>
        /// <remarks>
        /// This Logs Exceptions to Application Insights
        /// </remarks>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        private static void TrackException(Exception ex, string message = null)
        {
            try
            {
                Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ResetColor();

                        TelemetryClient.TrackTrace(message);
                    }

                    TelemetryClient.TrackException(ex);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{message} | Log Failure: {e.Message} | {e.Message}");
            }
        }

        #endregion //Trace

        #region LucetTrace
        /// <summary>
        /// Lucet Trace Logging
        /// </summary>
        /// <remarks>
        /// This Logs to On Premise
        /// </remarks>
        /// <param name="message"></param>
        private static void LucetTrace(string message)
        {
            try
            {
                Task.Run(() =>
                {
                    LucetLogger.Log(message);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{message} | Log Failure: {e.Message} | {e.Message}");
            }
        }

        /// <summary>
        /// Lucet Trace Logging
        /// </summary>
        /// <remarks>
        /// This Logs to On Premise
        /// </remarks>
        /// <param name="eventName"></param>
        /// <param name="messages"></param>
        private static void LucetTrace(string eventName, Dictionary<string, string> messages)
        {
            try
            {
                Task.Run(() =>
                {
                    LucetLogger.Log(eventName, messages);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{eventName} | Log Failure: {e.Message} | {e.Message}");
            }
        }

        /// <summary>
        /// Lucet Trace Logging
        /// </summary>
        /// <remarks>
        /// This Logs to On Premise
        /// </remarks>
        /// <param name="exception"></param>
        private static void LucetTrace(Exception exception)
        {
            try
            {
                Task.Run(() =>
                {
                    LucetLogger.Log(exception);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{exception.Message} | Log Failure: {e.Message} | {e.Message}");
            }
        }

        /// <summary>
        /// Lucet Trace Logging
        /// </summary>
        /// <remarks>
        /// This Logs to On Premise
        /// </remarks>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        private static void LucetTrace(string message, Exception exception)
        {
            try
            {
                Task.Run(() =>
                {
                    LucetLogger.Log(message, exception);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{message} | Exception: {exception.Message}| Log Failure: {e.Message} | {e.Message}");
            }
        }

        /// <summary>
        /// Lucet Trace Logging
        /// </summary>
        /// <remarks>
        /// This Logs to On Premise
        /// </remarks>
        /// <param name="message"></param>
        /// <param name="messages"></param>
        /// <param name="exception"></param>
        private static void LucetTrace(string message, Dictionary<string, string> messages, Exception exception)
        {
            try
            {
                Task.Run(() =>
                {
                    LucetLogger.Log(message, messages, exception);
                });
            }
            catch (Exception e)
            {
                SD.Trace.TraceError($"{message} | Exception: {exception.Message}| Log Failure: {e.Message} | {e.Message}");
            }
        }
        #endregion //LucetTrace

        #endregion //Private Methods
    }
}