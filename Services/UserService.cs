using Personal_Expense_Tracking.Data;
using Personal_Expense_Tracking.Models;
using SQLite;

namespace Personal_Expense_Tracking.Services
{
    public class UserService
    {
        private SQLiteConnection _db;

        // Constructor - Initialize the SQLite connection
        public UserService()
        {
            // Log the database path when the connection is created
            System.Diagnostics.Debug.WriteLine($"Database Path: {DataConfig.DatabasePath}");

            _db = new SQLiteConnection(DataConfig.DatabasePath); // Use DataConfig for path
            _db.CreateTable<User>(); // Create the table if it doesn't exist
        }

        // Insert user if not found or validate existing user
        public void EnsureUserExists(User user)
        {
            try
            {
                var existingUser = _db.Table<User>().FirstOrDefault(u => u.Username == user.Username);

                if (existingUser == null)
                {
                    _db.Insert(user); // Insert new user
                }
                else
                {
                    // Fetch and print username and password from the database

                    // Validate user credentials
                    if (VerifyUserCredentials(user.Username, user.Password))
                    {
                        System.Diagnostics.Debug.WriteLine("User validated successfully.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("User credentials are incorrect.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error ensuring user exists: {ex.Message}");
            }
        }

        // Validate user credentials
        public bool VerifyUserCredentials(string username, string password)
        {
            try
            {
                var user = _db.Table<User>().FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine("User not found during validation.");
                    return false; // User does not exist
                }

                // Check if password matches
                if (user.Password == password)
                {
                    System.Diagnostics.Debug.WriteLine("User validated successfully.");
                    return true;
                }

                System.Diagnostics.Debug.WriteLine("Invalid password.");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error and rethrow it
                System.Diagnostics.Debug.WriteLine($"An error occurred while verifying credentials: {ex.Message}");
                throw new Exception("An error occurred while verifying credentials.", ex);
            }
        }
    }
}
