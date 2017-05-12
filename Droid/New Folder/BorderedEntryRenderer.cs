using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Widget;
using Square;
using Square.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderedEntry), typeof(BorderedEntryRenderer))]
namespace Square.Droid
{
	public class BorderedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null)
			{
				var formsControl = Element as BorderedEntry;
				var nativeEditText = (EditText)Control;
				var shape = new ShapeDrawable(new RectShape());
				shape.Paint.Color = formsControl.BorderColor.ToAndroid();
				shape.Paint.SetStyle(Paint.Style.Stroke);
				nativeEditText.Background = shape;
			}
		}
	}
}
