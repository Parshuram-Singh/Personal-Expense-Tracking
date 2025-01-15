using Personal_Expense_Tracking.Models;
using Personal_Expense_Tracking.Data;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_Expense_Tracking.Services
{
    public class TransactionsService
    {
        public async Task InsertTransaction(Transactions transaction)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();
                    await Task.Run(() => db.Insert(transaction));
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error inserting transaction: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<List<Transactions>> GetAllTransactions()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();
                    return await Task.Run(() => db.Table<Transactions>().ToList());
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error fetching transactions: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task DeleteTransaction(int transactionId)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();

                    var transactionToDelete = await Task.Run(() => db.Table<Transactions>().FirstOrDefault(t => t.Id == transactionId));

                    if (transactionToDelete != null)
                    {
                        await Task.Run(() => db.Delete(transactionToDelete));
                        await Task.Run(() => db.Execute("DELETE FROM sqlite_sequence WHERE name = ?", typeof(Transactions).Name));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Transaction with ID {transactionId} not found.");
                    }
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deleting transaction: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<decimal> GetTotalInflow()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();
                    return await Task.Run(() =>
                        db.Table<Transactions>()
                          .Where(t => t.Type == TransactionsType.Credit)
                          .Sum(t => t.Amount));
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating total inflow: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<decimal> GetTotalOutflow()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();
                    return await Task.Run(() =>
                        db.Table<Transactions>()
                          .Where(t => t.Type == TransactionsType.Debit)
                          .Sum(t => t.Amount));
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating total outflow: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<decimal> GetTotalBalance()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();
                    db.CreateTable<Depts>();  // Ensure the Depts table is also created.

                    // Calculate total inflows (Credits)
                    var totalInflow = await Task.Run(() =>
                        db.Table<Transactions>()
                          .Where(t => t.Type == TransactionsType.Credit)
                          .Sum(t => t.Amount));

                    // Calculate total outflows (Debits)
                    var totalOutflow = await Task.Run(() =>
                        db.Table<Transactions>()
                          .Where(t => t.Type == TransactionsType.Debit)
                          .Sum(t => t.Amount));

                    // Calculate total debts (only Pending debts, if applicable)
                    var totalDebt = await Task.Run(() =>
                        db.Table<Depts>()
                          .Where(d => d.Status == Depts.DebtStatus.Pending) // Only include pending debts
                          .Sum(d => d.Amount));

                    // Balance formula: (inflows + debts) - outflows
                    return (totalInflow + totalDebt) - totalOutflow;
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating total balance: {ex.Message}");
                    throw;
                }
            }
        }
        public async Task<int> GetTotalNumberOfTransactions()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Transactions>();
                    System.Diagnostics.Debug.WriteLine($"Transactions count: ");
                    return await Task.Run(() => db.Table<Transactions>().Count());
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating total number of transactions: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
