using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel
{
	public class RequestResult
	{
		public RequestResult()
		{
			Errors = new List<string>();
		}

		public RequestResult(bool success)
		{
			Errors = new List<string>();
			Succeeded = success;
		}

		public RequestResult(IdentityResult result)
		{
			Errors = new List<string>();
			CovertIdentityResult(result);
		}

		public List<string> Errors { get; set; }

		public bool Succeeded { get; set; }

		public string NewKey { get; set; }

		public void CovertIdentityResult(IdentityResult result)
		{
			Succeeded = result.Succeeded;
			Errors = result.Errors.ToList();
		}
	}
}
