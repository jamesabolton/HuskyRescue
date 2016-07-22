using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace HuskyRescue.BusinessLogic.Identity
{
	public class RoleManager : RoleManager<IdentityRole>
	{
		public RoleManager(IRoleStore<IdentityRole, string> roleStore)
			: base(roleStore)
		{
		}

		//public static RoleManager Create(
		//	IdentityFactoryOptions<RoleManager> options, IOwinContext context)
		//{
		//	return new RoleManager(
		//		new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
		//}
	}
}
