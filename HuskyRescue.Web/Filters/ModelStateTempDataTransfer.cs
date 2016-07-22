using System.Web.Mvc;

namespace HuskyRescue.Web.Filters
{
	// http://weblogs.asp.net/rashid/asp-net-mvc-best-practices-part-1
	public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
	{
		protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
	}
}