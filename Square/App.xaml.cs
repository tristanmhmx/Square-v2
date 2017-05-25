using Xamarin.Forms;
using SQLite;
using Square.Interfaces;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using System;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;

namespace Square
{
	public partial class App : Application
	{
        public SQLiteAsyncConnection DatabaseConnection { get; set; }
        public Position CurrentPosition { get; set; } = new Position(19.432560, -99.132746);
        public static App Current { get; set; }

		public App()
		{
			InitializeComponent();

            Current = this;

            DatabaseConnection = DependencyService.Get<IDataService>().GetConnection();

			MainPage = new NavigationPage(new SquarePage());

            GetLocation();
		}

        async void GetLocation()
        {
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			var position = await locator.GetPositionAsync(10000);
            CurrentPosition = new Position(position.Latitude, position.Longitude);
			
            //Move map
			var mainpage = this.MainPage as NavigationPage;
			var mapPage = mainpage?.CurrentPage as SquarePage;
			mapPage?.CenterMap();

            await locator.StartListeningAsync(5, 10, true);
            locator.PositionChanged += Locator_PositionChanged;
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            CurrentPosition = new Position(e.Position.Latitude, e.Position.Longitude);
        }

        protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
