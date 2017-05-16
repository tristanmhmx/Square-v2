using System;
using Square.Interfaces;
using Xamarin.Forms;
using Square.iOS.Dependencies;
using SQLite;
using System.IO;

[assembly: Dependency(typeof(DataService))]
namespace Square.iOS.Dependencies
{
    public class DataService : IDataService
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, "SquareDb.db");
            var connection = new SQLiteAsyncConnection(path);
            return connection;
        }
    }
}
