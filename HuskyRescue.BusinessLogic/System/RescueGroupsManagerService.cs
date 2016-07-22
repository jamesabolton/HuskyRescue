using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HuskyRescue.ViewModel.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public class RescueGroupsManagerService : IRescueGroupsManagerService
	{
		private readonly ILogger _logger;

		public RescueGroupsManagerService(ILogger iLogger)
		{
			_logger = iLogger;
		}

		public List<RescueGroupAnimal> GetAdoptableHuskiesAsync()
		{
			var animals = new List<RescueGroupAnimal>();
			var data = new
			{
				apikey = AdminSystemSettings.GetSetting("RescueGroups-Api-Key"),
				objectType = "animals",
				objectAction = "publicSearch",
				search = new
				{
					resultStart = 0,
					resultLimit = 100,
					resultSort = "animalName",
					resultOrder = "asc",
					filters = new[]
					{
						new
						{
							fieldName = "animalStatus",
							operation = "equal",
							criteria = "Available"
						},
						new
						{
							fieldName = "animalOrgID",
							operation = "equal",
							criteria = "3427"
						}
					},
					fields = new[]
					         {
					         "animalID",
							"animalAltered",
							"animalBirthdate",
							"animalBreed",
							"animalColor",
							"animalCratetrained",
							"animalDescription",
							"animalEnergyLevel",
							"animalExerciseNeeds",
							"animalEyeColor",
							"animalGeneralAge",
							"animalHousetrained",
							"animalLeashtrained",
							"animalMixedBreed",
							"animalName",
							"animalNeedsFoster",
							"animalOKWithAdults",
							"animalOKWithCats",
							"animalOKWithDogs",
							"animalOKWithKids",
							"animalOthernames",
							"animalPictures",
							"animalRescueID",
							"animalSex",
							"animalSpecialneeds",
							"animalSpecialneedsDescription",
							"animalStatus",
							"animalSummary",
							"animalThumbnailUrl"
					         }
				}
			};

			var jsonData = JsonConvert.SerializeObject(data);

			var request = (HttpWebRequest)WebRequest.Create(AdminSystemSettings.GetSetting("RescueGroups-Api-Uri"));
			request.Method = "POST";
			request.ContentType = "application/json";
			var bytes = Encoding.UTF8.GetBytes(jsonData);
			request.ContentLength = bytes.Length;

			var requestStream = request.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);

			var result = string.Empty;
			try
			{
				var response = request.GetResponse();
				var stream = response.GetResponseStream();
				if (stream != null)
				{
					var reader = new StreamReader(stream);

					result = reader.ReadToEnd();
					stream.Dispose();
					reader.Dispose();
				}

				dynamic s = JObject.Parse(result);
				if (s.data == null) return animals;
				foreach (var animal in s.data)
				{
					if (animal == null) continue;
					var a = new RescueGroupAnimal
					{
						AnimalId = animal.Value.animalID != null ? animal.Value.animalID.Value : string.Empty,
						AnimalAltered = animal.Value.animalAltered != null ? animal.Value.animalAltered.Value : string.Empty,
						AnimalBirthdate = animal.Value.animalBirthdate != null ? animal.Value.animalBirthdate.Value : string.Empty,
						AnimalBreed = animal.Value.animalBreed != null ? animal.Value.animalBreed.Value : string.Empty,
						AnimalColor = animal.Value.animalColor != null ? animal.Value.animalColor.Value : string.Empty,
						AnimalCratetrained = animal.Value.animalCratetrained != null ? animal.Value.animalCratetrained.Value : string.Empty,
						AnimalDescription = animal.Value.animalDescription != null ? animal.Value.animalDescription.Value : string.Empty,
						AnimalEnergyLevel = animal.Value.animalEnergyLevel != null ? animal.Value.animalEnergyLevel.Value : string.Empty,
						AnimalExerciseNeeds = animal.Value.animalExerciseNeeds != null ? animal.Value.animalExerciseNeeds.Value : string.Empty,
						AnimalEyeColor = animal.Value.animalEyeColor != null ? animal.Value.animalEyeColor.Value : string.Empty,
						AnimalGeneralAge = animal.Value.animalGeneralAge != null ? animal.Value.animalGeneralAge.Value : string.Empty,
						AnimalHousetrained = animal.Value.animalHousetrained != null ? animal.Value.animalHousetrained.Value : string.Empty,
						AnimalLeashtrained = animal.Value.animalLeashtrained != null ? animal.Value.animalLeashtrained.Value : string.Empty,
						AnimalMixedBreed = animal.Value.animalMixedBreed != null ? animal.Value.animalMixedBreed.Value : string.Empty,
						AnimalName = animal.Value.animalName != null ? animal.Value.animalName.Value : string.Empty,
						AnimalNeedsFoster = animal.Value.animalNeedsFoster != null ? animal.Value.animalNeedsFoster.Value : string.Empty,
						AnimalOkWithAdults = animal.Value.animalOKWithAdults != null ? animal.Value.animalOKWithAdults.Value : string.Empty,
						AnimalOkWithCats = animal.Value.animalOKWithCats != null ? animal.Value.animalOKWithCats.Value : string.Empty,
						AnimalOkWithDogs = animal.Value.animalOKWithDogs != null ? animal.Value.animalOKWithDogs.Value : string.Empty,
						AnimalOkWithKids = animal.Value.animalOKWithKids != null ? animal.Value.animalOKWithKids.Value : string.Empty,
						AnimalOthernames = animal.Value.animalOthernames != null ? animal.Value.animalOthernames.Value : string.Empty,
						AnimalOrgId = animal.Value.animalOrgID != null ? animal.Value.animalOrgID.Value : string.Empty,
						AnimalRescueId = animal.Value.animalRescueID != null ? animal.Value.animalRescueID.Value : string.Empty,
						AnimalSex = animal.Value.animalSex != null ? animal.Value.animalSex.Value : string.Empty,
						AnimalSpecialneeds = animal.Value.animalSpecialneeds != null ? animal.Value.animalSpecialneeds.Value : string.Empty,
						AnimalSpecialneedsDescription = animal.Value.animalSpecialneedsDescription != null ? animal.Value.animalSpecialneedsDescription.Value : string.Empty,
						AnimalStatus = animal.Value.animalStatus != null ? animal.Value.animalStatus.Value : string.Empty,
						AnimalSummarypublic = animal.Value.animalSummarypublic != null ? animal.Value.animalSummarypublic.Value : string.Empty,
						AnimalThumbnailUrl = animal.Value.animalThumbnailUrl != null ? animal.Value.animalThumbnailUrl.Value : string.Empty
					};

					foreach (var picture in animal.Value.animalPictures)
					{
						a.AnimalPictures.Add(new RescueGroupPicture
						{
							MediaId = picture.mediaID != null ? picture.mediaID.Value : string.Empty,
							FileSize = picture.fileSize != null ? picture.fileSize.Value : string.Empty,
							ResolutionX = picture.resolutionX != null ? picture.resolutionX.Value : string.Empty,
							ResolutionY = picture.resolutionY != null ? picture.resolutionY.Value : string.Empty,
							MediaOrder = picture.mediaOrder != null ? picture.mediaOrder.Value : string.Empty,
							FileNameFullsize = picture.fileNameFullsize != null ? picture.fileNameFullsize.Value : string.Empty,
							FileNameThumbnail = picture.fileNameThumbnail != null ? picture.fileNameThumbnail.Value : string.Empty,
							UrlSecureFullsize = picture.urlSecureFullsize != null ? picture.urlSecureFullsize.Value : string.Empty,
							UrlSecureThumbnail = picture.urlSecureThumbnail != null ? picture.urlSecureThumbnail.Value : string.Empty,
							UrlInsecureFullsize = picture.urlInsecureFullsize != null ? picture.urlInsecureFullsize.Value : string.Empty,
							UrlInsecureThumbnail = picture.urlInsecureThumbnail != null ? picture.urlInsecureThumbnail.Value : string.Empty
						});
					}
					animals.Add(a);
				}
			}
			catch (WebException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (ProtocolViolationException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (InvalidOperationException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (NotSupportedException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}

			return animals;
		}

		public async Task<List<RescueGroupAnimal>> GetFosterableHuskiesAsync()
		{
			var animals = new List<RescueGroupAnimal>();
			var data = new
			{
				apikey = AdminSystemSettings.GetSetting("RescueGroups-Api-Key"),
				objectType = "animals",
				objectAction = "publicSearch",
				search = new
				{
					resultStart = 0,
					resultLimit = 100,
					resultSort = "animalName",
					resultOrder = "asc",
					filters = new[]
					{
						new
						{
							fieldName = "animalStatus",
							operation = "equal",
							criteria = "Available"
						},
						new
						{
							fieldName = "animalOrgID",
							operation = "equal",
							criteria = "3427"
						},
						new
						{
							fieldName = "animalNeedsFoster",
							operation = "equal",
							criteria = "Yes"
						}
					},
					fields = new[]
							{
							"animalID",
							"animalAltered",
							"animalBirthdate",
							"animalBreed",
							"animalColor",
							"animalCratetrained",
							"animalDescription",
							"animalEnergyLevel",
							"animalExerciseNeeds",
							"animalEyeColor",
							"animalGeneralAge",
							"animalHousetrained",
							"animalLeashtrained",
							"animalMixedBreed",
							"animalName",
							"animalNeedsFoster",
							"animalOKWithAdults",
							"animalOKWithCats",
							"animalOKWithDogs",
							"animalOKWithKids",
							"animalOthernames",
							"animalPictures",
							"animalRescueID",
							"animalSex",
							"animalSpecialneeds",
							"animalSpecialneedsDescription",
							"animalStatus",
							"animalSummary",
							"animalThumbnailUrl"
							}
				}
			};

			var jsonData = JsonConvert.SerializeObject(data);

			var request = (HttpWebRequest)WebRequest.Create(AdminSystemSettings.GetSetting("RescueGroups-Api-Uri"));
			request.Method = "POST";
			request.ContentType = "application/json";
			var bytes = Encoding.UTF8.GetBytes(jsonData);
			request.ContentLength = bytes.Length;

			var requestStream = await request.GetRequestStreamAsync();
			requestStream.Write(bytes, 0, bytes.Length);

			var result = string.Empty;
			try
			{
				var response = request.GetResponse();
				var stream = response.GetResponseStream();
				if (stream != null)
				{
					var reader = new StreamReader(stream);

					result = reader.ReadToEnd();
					stream.Dispose();
					reader.Dispose();
				}
			}
			catch (WebException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (ProtocolViolationException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (InvalidOperationException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (NotSupportedException ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error reading data from RescueGroups");
			}

			dynamic s = JObject.Parse(result);
			if (s.data == null) return animals;
			foreach (var animal in s.data)
			{
				if (animal == null) continue;
				var a = new RescueGroupAnimal
				        {
					        AnimalId = animal.Value.animalID != null ? animal.Value.animalID.Value : string.Empty,
					        AnimalAltered = animal.Value.animalAltered != null ? animal.Value.animalAltered.Value : string.Empty,
					        AnimalBirthdate = animal.Value.animalBirthdate != null ? animal.Value.animalBirthdate.Value : string.Empty,
					        AnimalBreed = animal.Value.animalBreed != null ? animal.Value.animalBreed.Value : string.Empty,
					        AnimalColor = animal.Value.animalColor != null ? animal.Value.animalColor.Value : string.Empty,
					        AnimalCratetrained = animal.Value.animalCratetrained != null ? animal.Value.animalCratetrained.Value : string.Empty,
					        AnimalDescription = animal.Value.animalDescription != null ? animal.Value.animalDescription.Value : string.Empty,
					        AnimalEnergyLevel = animal.Value.animalEnergyLevel != null ? animal.Value.animalEnergyLevel.Value : string.Empty,
					        AnimalExerciseNeeds = animal.Value.animalExerciseNeeds != null ? animal.Value.animalExerciseNeeds.Value : string.Empty,
					        AnimalEyeColor = animal.Value.animalEyeColor != null ? animal.Value.animalEyeColor.Value : string.Empty,
					        AnimalGeneralAge = animal.Value.animalGeneralAge != null ? animal.Value.animalGeneralAge.Value : string.Empty,
					        AnimalHousetrained = animal.Value.animalHousetrained != null ? animal.Value.animalHousetrained.Value : string.Empty,
					        AnimalLeashtrained = animal.Value.animalLeashtrained != null ? animal.Value.animalLeashtrained.Value : string.Empty,
					        AnimalMixedBreed = animal.Value.animalMixedBreed != null ? animal.Value.animalMixedBreed.Value : string.Empty,
					        AnimalName = animal.Value.animalName != null ? animal.Value.animalName.Value : string.Empty,
					        AnimalNeedsFoster = animal.Value.animalNeedsFoster != null ? animal.Value.animalNeedsFoster.Value : string.Empty,
					        AnimalOkWithAdults = animal.Value.animalOKWithAdults != null ? animal.Value.animalOKWithAdults.Value : string.Empty,
					        AnimalOkWithCats = animal.Value.animalOKWithCats != null ? animal.Value.animalOKWithCats.Value : string.Empty,
					        AnimalOkWithDogs = animal.Value.animalOKWithDogs != null ? animal.Value.animalOKWithDogs.Value : string.Empty,
					        AnimalOkWithKids = animal.Value.animalOKWithKids != null ? animal.Value.animalOKWithKids.Value : string.Empty,
					        AnimalOthernames = animal.Value.animalOthernames != null ? animal.Value.animalOthernames.Value : string.Empty,
					        AnimalOrgId = animal.Value.animalOrgID != null ? animal.Value.animalOrgID.Value : string.Empty,
					        AnimalRescueId = animal.Value.animalRescueID != null ? animal.Value.animalRescueID.Value : string.Empty,
					        AnimalSex = animal.Value.animalSex != null ? animal.Value.animalSex.Value : string.Empty,
					        AnimalSpecialneeds = animal.Value.animalSpecialneeds != null ? animal.Value.animalSpecialneeds.Value : string.Empty,
					        AnimalSpecialneedsDescription = animal.Value.animalSpecialneedsDescription != null ? animal.Value.animalSpecialneedsDescription.Value : string.Empty,
					        AnimalStatus = animal.Value.animalStatus != null ? animal.Value.animalStatus.Value : string.Empty,
					        AnimalSummarypublic = animal.Value.animalSummarypublic != null ? animal.Value.animalSummarypublic.Value : string.Empty,
					        AnimalThumbnailUrl = animal.Value.animalThumbnailUrl != null ? animal.Value.animalThumbnailUrl.Value : string.Empty
				        };

				foreach (var picture in animal.Value.animalPictures)
				{
					a.AnimalPictures.Add(new RescueGroupPicture
					                     {
						                     MediaId = picture.mediaID != null ? picture.mediaID.Value : string.Empty,
						                     FileSize = picture.fileSize != null ? picture.fileSize.Value : string.Empty,
						                     ResolutionX = picture.resolutionX != null ? picture.resolutionX.Value : string.Empty,
						                     ResolutionY = picture.resolutionY != null ? picture.resolutionY.Value : string.Empty,
						                     MediaOrder = picture.mediaOrder != null ? picture.mediaOrder.Value : string.Empty,
						                     FileNameFullsize = picture.fileNameFullsize != null ? picture.fileNameFullsize.Value : string.Empty,
						                     FileNameThumbnail = picture.fileNameThumbnail != null ? picture.fileNameThumbnail.Value : string.Empty,
						                     UrlSecureFullsize = picture.urlSecureFullsize != null ? picture.urlSecureFullsize.Value : string.Empty,
						                     UrlSecureThumbnail = picture.urlSecureThumbnail != null ? picture.urlSecureThumbnail.Value : string.Empty,
						                     UrlInsecureFullsize = picture.urlInsecureFullsize != null ? picture.urlInsecureFullsize.Value : string.Empty,
						                     UrlInsecureThumbnail = picture.urlInsecureThumbnail != null ? picture.urlInsecureThumbnail.Value : string.Empty
					                     });
				}
				animals.Add(a);
			}
			return animals;
		}

		public async Task<RescueGroupAnimal> GetHuskyProfileAsync(string id)
		{
			{
				var data = new
				{
					apikey = AdminSystemSettings.GetSetting("RescueGroups-Api-Key"),
					objectType = "animals",
					objectAction = "publicSearch",
					search = new
					{
						resultStart = 0,
						resultLimit = 1,
						resultSort = "animalName",
						resultOrder = "asc",
						filters = new[]
					{
						new
						{
							fieldName = "animalID",
							operation = "equal",
							criteria = id
						},
						new
						{
							fieldName = "animalOrgID",
							operation = "equal",
							criteria = "3427"
						},
						new
						{
							fieldName = "animalStatus",
							operation = "equal",
							criteria = "Available"
						}
					},
						fields = new[] { 
							"animalID",
							"animalAltered",
							"animalBirthdate",
							"animalBreed",
							"animalColor",
							"animalCratetrained",
							"animalDescription",
							"animalEnergyLevel",
							"animalExerciseNeeds",
							"animalEyeColor",
							"animalGeneralAge",
							"animalHousetrained",
							"animalLeashtrained",
							"animalMixedBreed",
							"animalName",
							"animalNeedsFoster",
							"animalOKWithAdults",
							"animalOKWithCats",
							"animalOKWithDogs",
							"animalOKWithKids",
							"animalOthernames",
							"animalPictures",
							"animalRescueID",
							"animalSex",
							"animalSpecialneeds",
							"animalSpecialneedsDescription",
							"animalStatus",
							"animalSummary"
						}
					}
				};

				var jsonData = JsonConvert.SerializeObject(data);

				var request = (HttpWebRequest)WebRequest.Create(AdminSystemSettings.GetSetting("RescueGroups-Api-Uri"));
				request.Method = "POST";
				request.ContentType = "application/json";
				var bytes = Encoding.UTF8.GetBytes(jsonData);
				request.ContentLength = bytes.Length;

				var requestStream = await request.GetRequestStreamAsync();
				requestStream.Write(bytes, 0, bytes.Length);

				var result = string.Empty;
				try
				{
					var response = request.GetResponse();
					var stream = response.GetResponseStream();
					if (stream != null)
					{
						var reader = new StreamReader(stream);

						result = reader.ReadToEnd();
						stream.Dispose();
						reader.Dispose();
					}
				}
				catch (WebException ex)
				{
					_logger.Error(ex, "Error reading data from RescueGroups");
				}
				catch (ProtocolViolationException ex)
				{
					_logger.Error(ex, "Error reading data from RescueGroups");
				}
				catch (InvalidOperationException ex)
				{
					_logger.Error(ex, "Error reading data from RescueGroups");
				}
				catch (NotSupportedException ex)
				{
					_logger.Error(ex, "Error reading data from RescueGroups");
				}
				catch (Exception ex)
				{
					_logger.Error(ex, "Error reading data from RescueGroups");
				}

				return JsonConvert.DeserializeObject<RescueGroupAnimal>(result);
			}
		}
	}
}
