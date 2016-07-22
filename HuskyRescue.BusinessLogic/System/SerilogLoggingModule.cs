using System.Linq;
using Autofac;
using Autofac.Core;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public class SerilogLoggingModule : Module
	{
		private static void AddILoggerToParameters(object sender, PreparingEventArgs e)
		{
			var t = e.Component.Activator.LimitType;
			e.Parameters = e.Parameters.Union(
				new[]
				{
					new ResolvedParameter((p, i) => p.ParameterType == typeof(ILogger), (p, i) => i.Resolve<ILogger>().ForContext(t)),
				});
		}

		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
		{
			registration.Preparing += AddILoggerToParameters;
		}
	}
}
