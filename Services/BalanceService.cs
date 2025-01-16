using Personal_Expense_Tracking.Models;
using SQLite;
using Personal_Expense_Tracking.Data;

namespace Personal_Expense_Tracking.Services
{
    internal class BalanceService
    {
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

                    // Get all pending debts
                    var pendingDebts = await Task.Run(() =>
                        db.Table<Depts>()
                          .Where(d => d.Status == Depts.DebtStatus.Pending)
                          .OrderBy(d => d.DueDate)  
                          .ToList());

                    decimal totalBalance = totalInflow - totalOutflow;  // Initial balance from inflows and outflows
                    decimal totalPendingDebtAmount = pendingDebts.Sum(d => d.Amount);
                    if (totalPendingDebtAmount > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Total Pending Debt Amount1: {totalPendingDebtAmount}");
                        foreach (var debt in pendingDebts)
                        {
                            if (totalInflow > 0)
                            {
                                if (totalInflow >= debt.Amount)
                                {
                                    totalInflow -= debt.Amount;  // Deduct from inflow
                                    debt.Status = Depts.DebtStatus.Cleared;
                                    debt.DueDate = DateTime.Now;  // Mark the date it was cleared
                                    db.Update(debt);  // Update the debt in the database
                                }
                                else
                                {
                                    debt.Amount -= totalInflow;
                                    totalInflow = 0;  // No more inflow left
                                    db.Update(debt);  // Update the debt with the remaining amount
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Recalculate remaining pending debt amount
                        var updatedDebts = await Task.Run(() =>
                            db.Table<Depts>()
                               .Where(d => d.Status == Depts.DebtStatus.Pending)
                               .ToList());

                        totalPendingDebtAmount = updatedDebts.Sum(d => d.Amount);
                        totalBalance = totalInflow - totalOutflow + totalPendingDebtAmount;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Total Pending Debt Amount2: {totalPendingDebtAmount}");

                        // If no pending debts, just add inflow to the balance
                        totalBalance = totalInflow - totalOutflow;
                    }


                    return totalBalance;
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error calculating total balance: {ex.Message}");
                    throw;
                }
            }
        }


    }
}
