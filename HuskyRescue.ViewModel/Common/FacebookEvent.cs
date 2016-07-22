using System;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Common
{
	public class FacebookEvent
	{
		public string Id { get; set; }

		public string Link
		{
			get { return "https://www.facebook.com/events/" + Id; }
		}

		public string Location { get; set; }

		public string Name { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public string GetFullLink()
		{
			var aLink = new TagBuilder("a");
			aLink.Attributes.Add("href", Link);
			aLink.SetInnerText(ToString());

			return aLink.ToString();
		}

		public override string ToString()
		{
			return Location + " from " + StartTime.ToShortTimeString() + " to " + EndTime.ToShortTimeString() + " on " + StartTime.Month + "/" + StartTime.Day + "/" + StartTime.Year;
		}
	}
}