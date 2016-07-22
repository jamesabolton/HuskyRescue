using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Common
{
	public static class Lists
	{
		public static string GetStateCode(int stateName)
		{
			var states = new List<KeyValuePair<int, string>>
			             {
							new KeyValuePair<int, string>(1, "AL"),
							new KeyValuePair<int, string>(2, "AK"),
							new KeyValuePair<int, string>(3, "AZ"),
							new KeyValuePair<int, string>(4, "AR"),
							new KeyValuePair<int, string>(5, "CA"),
							new KeyValuePair<int, string>(6, "CO"),
							new KeyValuePair<int, string>(7, "CT"),
							new KeyValuePair<int, string>(8, "DC"),
							new KeyValuePair<int, string>(9, "DE"),
							new KeyValuePair<int, string>(10, "FL"),
							new KeyValuePair<int, string>(11, "GA"),
							new KeyValuePair<int, string>(12, "HI"),
							new KeyValuePair<int, string>(13, "ID"),
							new KeyValuePair<int, string>(14, "IL"),
							new KeyValuePair<int, string>(15, "IN"),
							new KeyValuePair<int, string>(16, "IA"),
							new KeyValuePair<int, string>(17, "KS"),
							new KeyValuePair<int, string>(18, "KY"),
							new KeyValuePair<int, string>(19, "LA"),
							new KeyValuePair<int, string>(20, "ME"),
							new KeyValuePair<int, string>(21, "MD"),
							new KeyValuePair<int, string>(22, "MA"),
							new KeyValuePair<int, string>(23, "MI"),
							new KeyValuePair<int, string>(24, "MN"),
							new KeyValuePair<int, string>(25, "MS"),
							new KeyValuePair<int, string>(26, "MO"),
							new KeyValuePair<int, string>(27, "MT"),
							new KeyValuePair<int, string>(28, "NE"),
							new KeyValuePair<int, string>(29, "NV"),
							new KeyValuePair<int, string>(30, "NH"),
							new KeyValuePair<int, string>(31, "NJ"),
							new KeyValuePair<int, string>(32, "NM"),
							new KeyValuePair<int, string>(33, "NY"),
							new KeyValuePair<int, string>(34, "NC"),
							new KeyValuePair<int, string>(35, "ND"),
							new KeyValuePair<int, string>(36, "OH"),
							new KeyValuePair<int, string>(37, "OK"),
							new KeyValuePair<int, string>(38, "OR"),
							new KeyValuePair<int, string>(39, "PA"),
							new KeyValuePair<int, string>(40, "RI"),
							new KeyValuePair<int, string>(41, "SC"),
							new KeyValuePair<int, string>(42, "SD"),
							new KeyValuePair<int, string>(43, "TN"),
							new KeyValuePair<int, string>(44, "TX"),
							new KeyValuePair<int, string>(45, "UT"),
							new KeyValuePair<int, string>(46, "VT"),
							new KeyValuePair<int, string>(47, "VA"),
							new KeyValuePair<int, string>(48, "WA"),
							new KeyValuePair<int, string>(49, "WV"),
							new KeyValuePair<int, string>(50, "WI"),
							new KeyValuePair<int, string>(51, "WY")
						 };

			return states.Single(s => s.Key == stateName).Value;
		}

		public static List<SelectListItem> GetStateList()
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "", Value = ""},
				           new SelectListItem {Value = "1", Text = "Alaska"},
				           new SelectListItem {Value = "2", Text = "Alabama"},
				           new SelectListItem {Value = "3", Text = "Arkansas"},
				           new SelectListItem {Value = "4", Text = "Arizona "},
				           new SelectListItem {Value = "5", Text = "California "},
				           new SelectListItem {Value = "6", Text = "Colorado"},
				           new SelectListItem {Value = "7", Text = "Connecticut"},
				           new SelectListItem {Value = "8", Text = "District Of Columbia"},
				           new SelectListItem {Value = "9", Text = "Delaware"},
				           new SelectListItem {Value = "10", Text = "FlorIDa"},
				           new SelectListItem {Value = "11", Text = "Georgia"},
				           new SelectListItem {Value = "12", Text = "Hawaii"},
				           new SelectListItem {Value = "13", Text = "Iowa"},
				           new SelectListItem {Value = "14", Text = "Idaho"},
				           new SelectListItem {Value = "15", Text = "Illinois"},
				           new SelectListItem {Value = "16", Text = "Indiana"},
				           new SelectListItem {Value = "17", Text = "Kansas"},
				           new SelectListItem {Value = "18", Text = "Kentucky"},
				           new SelectListItem {Value = "19", Text = "Louisiana"},
				           new SelectListItem {Value = "20", Text = "Massachusetts"},
				           new SelectListItem {Value = "21", Text = "Maryland"},
				           new SelectListItem {Value = "22", Text = "Maine"},
				           new SelectListItem {Value = "23", Text = "Michigan"},
				           new SelectListItem {Value = "24", Text = "Minnesota"},
				           new SelectListItem {Value = "25", Text = "Missouri"},
				           new SelectListItem {Value = "26", Text = "Mississippi"},
				           new SelectListItem {Value = "27", Text = "Montana"},
				           new SelectListItem {Value = "28", Text = "North Carolina"},
				           new SelectListItem {Value = "29", Text = "North Dakota"},
				           new SelectListItem {Value = "30", Text = "Nebraska"},
				           new SelectListItem {Value = "31", Text = "New Hampshire"},
				           new SelectListItem {Value = "32", Text = "New Jersey"},
				           new SelectListItem {Value = "33", Text = "New Mexico"},
				           new SelectListItem {Value = "34", Text = "Nevada"},
				           new SelectListItem {Value = "35", Text = "New York"},
				           new SelectListItem {Value = "36", Text = "Ohio"},
				           new SelectListItem {Value = "37", Text = "Oklahoma"},
				           new SelectListItem {Value = "38", Text = "Oregon"},
				           new SelectListItem {Value = "39", Text = "Pennsylvania"},
				           new SelectListItem {Value = "40", Text = "Rhode Island"},
				           new SelectListItem {Value = "41", Text = "South Carolina"},
				           new SelectListItem {Value = "42", Text = "South Dakota"},
				           new SelectListItem {Value = "43", Text = "Tennessee"},
				           new SelectListItem {Value = "44", Text = "Texas"},
				           new SelectListItem {Value = "45", Text = "Utah"},
				           new SelectListItem {Value = "46", Text = "Virginia"},
				           new SelectListItem {Value = "47", Text = "Vermont"},
				           new SelectListItem {Value = "48", Text = "Washington"},
				           new SelectListItem {Value = "49", Text = "Wisconsin"},
				           new SelectListItem {Value = "50", Text = "West Virginia"},
				           new SelectListItem {Value = "51", Text = "Wyoming"},
				           new SelectListItem {Value = "52", Text = "Unknown"}
			           };
			return list;
		}

		public static List<SelectListItem> GetAddressTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Primary", Value = "1"},
				           new SelectListItem {Text = "Mailing", Value = "2"},
				           new SelectListItem {Text = "Other", Value = "3"},
				           new SelectListItem {Text = "Billing", Value = "4"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetResidenceOwnershipTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Own", Value = "1"},
				           new SelectListItem {Text = "Rent", Value = "2"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetResidencTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "House", Value = "1"},
				           new SelectListItem {Text = "Apartment", Value = "2"},
				           new SelectListItem {Text = "Mobile Home", Value = "3"},
				           new SelectListItem {Text = "Duplex", Value = "4"},
				           new SelectListItem {Text = "Condo", Value = "5"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetResidenceCoverageTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "per pet", Value = "1"},
				           new SelectListItem {Text = "per household", Value = "2"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetStudentTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Full Time", Value = "1"},
				           new SelectListItem {Text = "Part Time", Value = "2"}
			           };

			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "N/A", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetSexTypeList(bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Male", Value = "1"},
				           new SelectListItem {Text = "Female", Value = "2"}
			           };
			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetEmailTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Home", Value = "1"},
				           new SelectListItem {Text = "Work", Value = "2"},
				           new SelectListItem {Text = "School", Value = "3"},
				           new SelectListItem {Text = "Other", Value = "4"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetPhoneTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Home", Value = "1"},
				           new SelectListItem {Text = "Work", Value = "2"},
				           new SelectListItem {Text = "Mobile", Value = "3"},
				           new SelectListItem {Text = "Fax", Value = "4"},
						   new SelectListItem {Text = "Other", Value = "5"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}

		public static List<SelectListItem> GetContactTypeList(bool includedUnknown = false, bool includeBlank = true)
		{
			var list = new List<SelectListItem>
			           {
				           new SelectListItem {Text = "Adoption related", Value = "1"},
				           new SelectListItem {Text = "Surrendering a Dog", Value = "2"},
				           new SelectListItem {Text = "Found a stray husky", Value = "3"},
				           new SelectListItem {Text = "Volunteer", Value = "4"},
						   new SelectListItem {Text = "Event Information", Value = "5"},
						   new SelectListItem {Text = "General Question", Value = "6"}
			           };
			if (includedUnknown)
			{
				list.Add(new SelectListItem { Text = "Unknown", Value = "0" });
			}

			if (includeBlank)
			{
				list.Insert(0, new SelectListItem { Text = "", Value = "" });
			}
			return list;
		}
	}
}
