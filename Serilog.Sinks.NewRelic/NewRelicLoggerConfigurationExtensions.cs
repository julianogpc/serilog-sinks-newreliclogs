﻿using System;
using System.Configuration;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.NewRelic
{
    public static class NewRelicLoggerConfigurationExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="loggerSinkConfiguration"></param>
        /// <param name="restrictedToMinimumLevel"></param>
        /// <param name="batchPostingLimit"></param>
        /// <param name="period"></param>
        /// <param name="applicationName">Application name in NewRelic. This can be either supplied here or through "NewRelic.AppName" appSettings</param>
        /// <param name="customEventName">The name of a custom event name emitted by logging events Warning, Information, Debug, Verbose. Defaults to "Serilog".</param>
        /// <returns></returns>
        public static LoggerConfiguration NewRelic(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            string applicationName = null,
            string customEventName = "Serilog",
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = NewRelicSink.DefaultBatchPostingLimit,
            TimeSpan? period = null
            )
        {
            if (loggerSinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerSinkConfiguration));
            }

            if (string.IsNullOrEmpty(applicationName))
            {
                applicationName = ConfigurationManager.AppSettings["NewRelic.AppName"];
                if (string.IsNullOrEmpty(applicationName))
                {
                    throw new ArgumentException("Must supply an application name eithe as parameter or as appSetting", nameof(applicationName));
                }
            }

            var defaultedPeriod = period ?? NewRelicSink.DefaultPeriod;

            ILogEventSink sink = new NewRelicSink(applicationName, batchPostingLimit, defaultedPeriod, customEventName);

            return loggerSinkConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}