using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Expense_Tracking.Models;
using Personal_Expense_Tracking.Data;
using SQLite;
using static Personal_Expense_Tracking.Models.Depts;

namespace Personal_Expense_Tracking.Services
{
    internal class DeptService
    {
        public async Task InsertDebt(Depts debt)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>();
                    await Task.Run(() => db.Insert(debt)); // Insert debt into the database
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error inserting debt: {ex.Message}");
                    throw;
                }
            }
        }

        // Method to get the total debt amount
        public async Task<decimal> GetTotalDeptAmount()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>();
                    return await Task.Run(() =>
                        db.Table<Depts>().Sum(d => d.Amount)); // Sum all debt amounts
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating total debt amount: {ex.Message}");
                    throw;
                }
            }
        }

        // Method to retrieve all debts from the database
        public async Task<List<Depts>> GetAllDebts()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>();
                    return await Task.Run(() => db.Table<Depts>().ToList()); // Fetch all debts
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading debts: {ex.Message}");
                    throw;
                }
            }
        }

        // Method to fetch a specific debt by its ID
        public async Task<Depts> GetDebtById(int debtId)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>(); // Ensure the table is created
                    return await Task.Run(() => db.Find<Depts>(debtId)); // Retrieve debt by ID asynchronously
                }
                catch (SQLiteException ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Error fetching debt: {ex.Message}");
                    throw;  // Rethrow exception to propagate the error
                }
            }
        }

        // Method to update the status of a debt
        public async Task UpdateDebt(Depts debt)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>();

                    // Check if debt is already cleared
                    if (debt.Status == Depts.DebtStatus.Pending)
                    {
                        // If cleared, update debt status to Cleared
                        debt.Status = Depts.DebtStatus.Cleared;
                        await Task.Run(() => db.Update(debt));  // Update the debt status to Cleared
                    }
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error updating debt: {ex.Message}");
                    throw;
                }
            }
        }


        // Method to delete a specific debt
        public async Task DeleteDebt(Depts debt)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>(); // Ensure the table is created
                    await Task.Run(() => db.Delete(debt)); // Delete the debt asynchronously
                }
                catch (SQLiteException ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Error deleting debt: {ex.Message}");
                    throw;  // Rethrow exception to propagate the error
                }
            }
        }


        public async Task<decimal> GetTotalPendingDeptAmount(decimal creditAmount)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>();
                    var pendingDebts = await Task.Run(() =>
                        db.Table<Depts>()
                          .Where(d => d.Status == Depts.DebtStatus.Pending)
                          .OrderBy(d => d.Amount)
                          .ToList()
                    );

                    decimal remainingCredit = creditAmount;

                    foreach (var debt in pendingDebts)
                    {
                        if (remainingCredit >= debt.Amount)
                        {
                            remainingCredit -= debt.Amount;
                            debt.Status = Depts.DebtStatus.Cleared;
                            await Task.Run(() => db.Update(debt));
                        }
                        else
                        {
                            break; // Stop when credit is insufficient to cover further debts
                        }
                    }

                    // Return the remaining credit or the amount of debt still pending
                    return pendingDebts.Where(d => d.Status == Depts.DebtStatus.Pending).Sum(d => d.Amount);
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating and clearing pending debt amount: {ex.Message}");
                    throw;
                }
            }
        }


    }
}
