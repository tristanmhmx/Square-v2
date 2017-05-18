using System;
using SQLite;
using Square.Droid.Dependencies;
using Square.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataService))]
namespace Square.Droid.Dependencies
{
    public class DataService : IDataService
    {
        public SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "square.db"));
        }
    }
}
