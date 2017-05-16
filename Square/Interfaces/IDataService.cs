using SQLite;
namespace Square.Interfaces
{
    public interface IDataService
    {
        SQLiteAsyncConnection GetConnection();
    }
}
