using System;
using Serilog;
using Serilog.Events;
//using Serilog.Extras.Web;
//using Serilog.Extras.Web.Enrichers;
using SerilogWeb.Classic;
using SerilogWeb.Classic.Enrichers;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFile;

namespace HuskyRescue.BusinessLogic
{
	public class SerilogConfig
	{
		public static ILogger CreateLogger()
		{
			var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

			var config = new LoggerConfiguration().
				MinimumLevel.Verbose().
				WriteTo.Sink(
					new RollingFileSink(path + "\\log-debug-{Date}.json", new JsonFormatter(renderMessage: true), null, null),
					LogEventLevel.Debug).
				WriteTo.Sink(
					new RollingFileSink(path + "\\log-warning-{Date}.json", new JsonFormatter(renderMessage: true), null, null),
					LogEventLevel.Warning).
				WriteTo.Seq("http://localhost:5341/").
				WriteTo.Trace();

			InitialiseGlobalContext(config);
			
			return config.CreateLogger();
		}

		public static LoggerConfiguration InitialiseGlobalContext(LoggerConfiguration configuration)
		{
			return configuration
				.Enrich.WithProperty("ApplicationName", typeof(SerilogConfig).Assembly.GetName().Name)
				.Enrich.WithProperty("AppDomain", AppDomain.CurrentDomain)
				//.Enrich.With<HttpRequestIdEnricher>()
				//.Enrich.With<HttpRequestRawUrlEnricher>()
				//.Enrich.With<HttpRequestClientHostIPEnricher>()
				//.Enrich.With<HttpRequestUserAgentEnricher>()
				//.Enrich.With<UserNameEnricher>()

				.Enrich.With(new HttpRequestIdEnricher(),
							 new HttpRequestRawUrlEnricher(),
							 new HttpRequestClientHostIPEnricher(),
							 new HttpRequestUserAgentEnricher(),
							 new UserNameEnricher())

				// this ensures that calls to LogContext.PushProperty will cause the logger to be enriched
				.Enrich.FromLogContext();
		}
	}
}
