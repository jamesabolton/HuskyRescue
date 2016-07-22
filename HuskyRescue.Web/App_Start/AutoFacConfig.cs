using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.BusinessLogic.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Serilog;

namespace HuskyRescue.Web.App_Start
{
	public class AutoFacConfig
	{
		public static IContainer Startup()
		{
			var builder = new ContainerBuilder();

			// Register your MVC controllers
			// Scan for all controllers
			builder.RegisterControllers(typeof(MvcApplication).Assembly);
			// Manually add controllers:
			//builder.RegisterType<HomeController>().InstancePerRequest();


			builder.RegisterType<ResourceManagerService>().As<IResourceManagerService>().InstancePerDependency();
			builder.RegisterType<IdentityManagerService>().As<IIdentityManagerService>().InstancePerDependency();
			builder.RegisterType<AdminSystemSettingsManagerService>().As<IAdminSystemSettingsManagerService>().InstancePerDependency();
			builder.RegisterType<ApplicationManagerService>().As<IApplicationManagerService>().InstancePerDependency();
			builder.RegisterType<AdminRolesManagerService>().As<IAdminRolesManagerService>().InstancePerDependency();
			builder.RegisterType<AdminUserManagerService>().As<IAdminUserManagerService>().InstancePerDependency();
			builder.RegisterType<FormSerivce>().As<IFormSerivce>().InstancePerDependency();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerDependency();
			//builder.RegisterType<AdminSystemSettings>().PropertiesAutowired();
			builder.RegisterType<BraintreePaymentService>().As<IBraintreePaymentService>().InstancePerDependency();
			builder.RegisterType<FacebookManagerService>().As<IFacebookManagerService>().InstancePerDependency();
			builder.RegisterType<RescueGroupsManagerService>().As<IRescueGroupsManagerService>().InstancePerDependency();
			builder.RegisterType<GolfEventManagerService>().As<IGolfEventManagerService>().InstancePerDependency();
			builder.RegisterType<AdminEventsManagerService>().As<IAdminEventsManagerService>().InstancePerDependency();
			builder.RegisterType<AdminOrganizationsManagerService>().As<IAdminOrganizationsManagerService>().InstancePerDependency();
			builder.RegisterType<RoughRidersEventManagerService>().As<IRoughRidersEventManagerService>().InstancePerDependency();
			builder.RegisterType<AdminEventsSponsorshipManagerService>().As<IAdminEventsSponsorshipManagerService>().InstancePerDependency();
			builder.RegisterType<AdminEventsSponsorManagerService>().As<IAdminEventsSponsorManagerService>().InstancePerDependency();
			
			builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
			builder.Register<UserStore<ApplicationUser>>(a => new UserStore<ApplicationUser>(new ApplicationDbContext())).AsImplementedInterfaces().InstancePerLifetimeScope();
			builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>();
			builder.RegisterType<UserManager>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<SignInManager>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<RoleManager>().AsSelf().InstancePerLifetimeScope();
			builder.Register<Microsoft.Owin.Security.IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication);

			// OPTIONAL: Register model binders that require DI.
			//builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
			//builder.RegisterModelBinderProvider();

			// OPTIONAL: Register web abstractions like HttpContextBase.
			builder.RegisterModule<AutofacWebTypesModule>();

			// OPTIONAL: Enable property injection in view pages.
			//builder.RegisterSource(new ViewRegistrationSource());

			// OPTIONAL: Enable property injection into action filters.
			builder.RegisterFilterProvider();

			

			builder.Register(c => SerilogConfig.CreateLogger())
				   .As<ILogger>()
				   .SingleInstance();

			builder.RegisterModule<SerilogLoggingModule>();

			// Set the dependency resolver to be Autofac.
			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			return container;
		}
	}
}