using SQLite;

namespace Personal_Expense_Tracking.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }  

        [MaxLength(50), Unique, NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Password { get; set; } // Store hashed password instead of plain text.
        public string PreferredCurrency { get; set; } = "USD";

    }
}
