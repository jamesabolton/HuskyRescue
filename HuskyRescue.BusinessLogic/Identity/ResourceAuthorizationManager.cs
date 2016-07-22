using System.Linq;
using System.Security.Claims;
using System.Threading;
using Serilog;

namespace HuskyRescue.BusinessLogic.Identity
{
	public class ResourceAuthorizationManager : ClaimsAuthorizationManager
	{
		private readonly ILogger _logger;
		private readonly IResourceManagerService _resourceManagerService;

		public ResourceAuthorizationManager(ILogger iLogger, IResourceManagerService iResourceManagerService)
		{
			_logger = iLogger;
			_resourceManagerService = iResourceManagerService;
		}

		public override bool CheckAccess(AuthorizationContext context)
		{
			var controller = context.Resource.First().Value;
			var action = context.Action.First().Value;
			//TODO figure out how to get action request type: get or post
			var requestType = "get";


			//Get the current claims principal
			var prinicpal = (ClaimsPrincipal) Thread.CurrentPrincipal;
			//Make sure they are authenticated
			if (!prinicpal.Identity.IsAuthenticated)
				return false;
			//Get the roles from the claims
			var roles = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
			//Check if they are authorized
			return _resourceManagerService.Authorize(string.Join("-", controller, action, requestType), roles);
		}
	}
}
