using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Square.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();

			LoadApplication(new App());

            if(UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert |
                    UIUserNotificationType.Badge |
                    UIUserNotificationType.Sound, new NSSet());
                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }else{
                var notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

			return base.FinishedLaunching(app, options);
		}

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var token = deviceToken.Description;
            if(!string.IsNullOrEmpty(token)){
                token = token.Trim('<').Trim('>');
            }
			//Send token to service (azure, amazon)
		}
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            base.FailedToRegisterForRemoteNotifications(application, error);
        }
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 1;
            base.ReceivedRemoteNotification(application, userInfo);
        }
	}
}
