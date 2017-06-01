using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using Square.Droid.Services;

namespace Square.Droid
{
	[Activity(Label = "Square.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

            Xamarin.FormsMaps.Init(this, bundle);

			LoadApplication(new App());

            if(IsPlayServicesAvailable())
            {
                var intent = new Intent(this, typeof(NotificationsIntentService));
                StartService(intent);
            }
		}

        bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if(resultCode != ConnectionResult.Success)
            {
                if(GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    return false;
                }
                else{
                    return false;
                }
            }
            else{
                return true;
            }
        }
	}
}
