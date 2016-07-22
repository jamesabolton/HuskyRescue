using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic.Identity;
using HuskyRescue.ViewModel.Admin.ResourceManager;
using HuskyRescue.Web.Filters;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	[AllowAnonymous]
	public class AdminIdentityResourcesController : BaseController
	{
		private readonly ILogger _logger;

		public IResourceManagerService ResourceManagerService { get; set; }

		public AdminIdentityResourcesController(ILogger iLogger, IResourceManagerService resourceManagerService) : base(iLogger)
		{
			_logger = iLogger;
			ResourceManagerService = resourceManagerService;
		}

		// GET: ResourceManager
		public async Task<ActionResult> Index()
		{
			var resources = await ResourceManagerService.GetResourceListAsync();
			var resourceCompare = new ResourceIndexCompare();
			resources.Sort(resourceCompare);
			return View(resources);
		}

		// GET: ResourceManager/Details/5
		public async Task<ActionResult> Details(string id)
		{
			var resource = await ResourceManagerService.GetResourceDetailAsync(id);
			return View(resource);
		}

		// GET: ResourceManager/Create
		[ImportModelStateFromTempData]
		public ActionResult Create()
		{
			return View(new ResourceCreate());
		}

		// POST: ResourceManager/Create
		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public async Task<ActionResult> Create(ResourceCreate resourceCreate)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await ResourceManagerService.AddResourceAsync(resourceCreate);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						return RedirectToAction("Create");
					}
				}
			}
			catch (DataException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("Create");
		}

		// GET: ResourceManager/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ResourceManager/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: ResourceManager/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ResourceManager/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
