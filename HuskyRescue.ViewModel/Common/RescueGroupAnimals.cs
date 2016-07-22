using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HuskyRescue.ViewModel.Common
{
	public class RescueGroupAnimals
	{
		public RescueGroupAnimals()
		{
			Animals = new List<RescueGroupAnimal>();
		}

		public List<RescueGroupAnimal> Animals { get; set; }

		// convert Animals property to JSON string
		public string ToJsonString(bool format = false)
		{
			var formating = format ? Formatting.Indented : Formatting.None;
			return Animals == null ? string.Empty : JsonConvert.SerializeObject(Animals, formating);
		}

		/// <summary>
		/// Count of the animals padded to have an number divisible by 8
		/// </summary>
		public int Count
		{
			get
			{
				var count = Animals.Count;
				var mod = count % 8;
				if (mod == 0) return count;

				return (8 - mod) + count;
			}
		}
	}
}
