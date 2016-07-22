using System;
using System.Collections.Generic;
using Facebook;
using HuskyRescue.ViewModel.Common;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public class FacebookManagerService : IFacebookManagerService
	{
		private readonly ILogger _logger;

		public FacebookManagerService(ILogger iLogger)
		{
			_logger = iLogger;
		}

		public List<FacebookEvent> GetEvents()
		{
			var fbEventList = new List<FacebookEvent>();

			try
			{
				// get access token
				var facebookClientAuth = new FacebookClient();
				dynamic oauth = facebookClientAuth.Get("oauth/access_token", new
				                                                             {
					                                                             client_id =
					                                                             AdminSystemSettings.GetSetting("facebook-app-id"),
					                                                             client_secret =
					                                                             AdminSystemSettings.GetSetting("facebook-app-secret"),
					                                                             grant_type = "client_credentials"
				                                                             });

				// create client to access events
				var facebookClientApp = new FacebookClient
				                        {
					                        AppId = AdminSystemSettings.GetSetting("facebook-app-id"),
					                        AppSecret = AdminSystemSettings.GetSetting("facebook-app-secret"),
					                        IsSecureConnection = true,
					                        AccessToken = oauth.access_token
				                        };

				// retrieve events
				dynamic data = facebookClientApp.Get("texashuskyrescue/events");
				//var fbEvents = data.data;

				// create event view model objects
				foreach (var fbEvent in data.data)
				{
					var facebookEvent = new FacebookEvent
					                    {
						                    Id = fbEvent.id,
						                    Location = fbEvent.location,
						                    Name = fbEvent.name,
						                    StartTime = Convert.ToDateTime(fbEvent.start_time),
						                    EndTime = Convert.ToDateTime(fbEvent.end_time)
					                    };
					if (facebookEvent.StartTime.Date >= DateTime.Today)
					{
						fbEventList.Add(facebookEvent);
					}
				}
				fbEventList.Sort(new FacebookEventComparer());
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error loading events from Facebook");
			}

			return fbEventList;
		}
	}
}
