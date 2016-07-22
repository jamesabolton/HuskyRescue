using System.Collections.Generic;
using System.ComponentModel;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public class ResourceIndex : IResourceIndex
	{
		public string Id { get; set; }

		public string Name { get; set; }

		[DisplayName("Allowed Access")]
		public string OperationNames { get; set; }
	}

	public class ResourceIndexCompare : IComparer<ResourceIndex>
	{
		public int Compare(ResourceIndex x, ResourceIndex y)
		{
			if (x.Name == null)
			{
				if (y.Name == null)
				{
					// If x is null and y is null, they're equal.  
					return 0;
				}
				else
				{
					// If x is null and y is not null, y is greater.  
					return -1;
				}
			}
			else
			{
				// If x is not null... 
				if (y.Name == null)
				// ...and y is null, x is greater.
				{
					return 1;
				}
				else
				{
					// ...and y is not null, compare the lengths of the two strings. 
					//int retval = x.Name.Length.CompareTo(y.Name.Length);

					//if (retval != 0)
					//{
					//	// If the strings are not of equal length, the longer string is greater. 
					//	return retval;
					//}
					//else
					//{
						// If the strings are of equal length, sort them with ordinary string comparison. 
					// ReSharper disable once StringCompareToIsCultureSpecific
						return x.Name.CompareTo(y.Name);
					//}
				}
			}
		}
	}
}