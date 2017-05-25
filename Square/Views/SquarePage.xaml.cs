using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Square
{
	public partial class SquarePage : ContentPage
	{
		public SquarePage()
		{
			InitializeComponent();
		}
        public void CenterMap()
        {
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(App.Current.CurrentPosition.Latitude, App.Current.CurrentPosition.Longitude), 
                Distance.FromKilometers(1)));
        }
	}
}
