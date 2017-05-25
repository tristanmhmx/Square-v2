using System;
using Square.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ResolutionGroupName("Alset")]
[assembly: ExportEffect(typeof(BlueButtonEffect), nameof(BlueButtonEffect))]
namespace Square.iOS
{
	public class BlueButtonEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			Control.BackgroundColor = UIColor.FromRGB(0, 148, 207);
			var button = Control as UIButton;
			button.SetTitleColor(UIColor.White, UIControlState.Normal);
		}

		protected override void OnDetached()
		{
			Control.BackgroundColor = UIColor.White;
			var button = Control as UIButton;
			button.SetTitleColor(UIColor.Black, UIControlState.Normal);
		}
	}
}
