using Xamarin.Forms;
using SQLite;
using Square.Interfaces;

namespace Square
{
	public partial class App : Application
	{
        public SQLiteAsyncConnection DatabaseConnection { get; set; }
        public static App Current { get; set; }

		public App()
		{
			InitializeComponent();

            Current = this;

            DatabaseConnection = DependencyService.Get<IDataService>().GetConnection();

			MainPage = new NavigationPage(new SquarePage());
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
