using System;
using Square.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Alset")]
[assembly: ExportEffect(typeof(BlueButtonEffect), nameof(BlueButtonEffect))]
namespace Square.Droid
{
	public class BlueButtonEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			Control.SetBackgroundColor(Color.FromRgb(0, 148, 207).ToAndroid());
		}

		protected override void OnDetached()
		{
			Control.SetBackgroundColor(Android.Graphics.Color.White);
		}
	}
}
