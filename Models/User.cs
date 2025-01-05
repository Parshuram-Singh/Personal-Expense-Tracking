using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Personal_Expense_Tracking.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Full_name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
