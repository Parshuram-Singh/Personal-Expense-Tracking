using System;
using SQLite;

namespace Personal_Expense_Tracking.Models
{
    public class Dept
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; } 

        [NotNull]
        public string Source { get; set; } 

        [NotNull]
        public decimal Amount { get; set; } 

        [NotNull]
        public DateTime DueDate { get; set; } 

        public DebtStatus Status { get; set; } = DebtStatus.Pending; 

        public Dept() { }
    }

    public enum DebtStatus
    {
        Pending,
        Cleared
    }
}
