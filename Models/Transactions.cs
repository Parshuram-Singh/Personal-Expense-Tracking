using System;
using SQLite;


namespace Personal_Expense_Tracking.Models
{
    public class Transactions
    {
        [PrimaryKey, AutoIncrement ,NotNull]
        public int Id { get; set; } // Primary Key, will auto-increment
        public string Title { get; set; }
        public string Notes { get; set; } = string.Empty; // Additional remarks about the transaction
        public TransactionsType Type { get; set; } // Enum for transaction type
        public decimal Amount { get; set; } // Use decimal for currency precision
        public DateTime Date { get; set; } = DateTime.Now; // Default to current date
        public string Tag { get; set; } = string.Empty; // Optional tag
        public TransactionsStatus Status { get; set; } = TransactionsStatus.Completed; // Transaction status enum

        public Transactions() { }
    }

    public enum TransactionsType
    {
        Credit,
        Debit
    }

    public enum TransactionsStatus
    {
        Pending,
        Completed,
        Failed
    }
}
