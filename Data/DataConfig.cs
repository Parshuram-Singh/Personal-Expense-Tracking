using System.IO;
using SQLite;
using Microsoft.Maui.Storage;

namespace Personal_Expense_Tracking.Data
{
    public class DataConfig
    {
        public const string DatabaseFilename = "ExpenseTracker.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // Open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // Create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // Enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public static SQLiteConnection GetDatabaseConnection()
        {
            // Return a new SQLite connection with the configured path and flags
            return new SQLiteConnection(DatabasePath, Flags);
        }
    }
}
