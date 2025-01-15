using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Expense_Tracking.Models;
using Personal_Expense_Tracking.Services;
using Personal_Expense_Tracking.Data;

namespace Personal_Expense_Tracking.ViewModels
{
    internal class DeptsViewModel
    {
        private DeptService _deptService;

        // This property will hold all debts
        public List<Depts> AllDebts { get; set; }

        public DeptsViewModel()
        {
            _deptService = new DeptService();
            AllDebts = new List<Depts>(); // Initialize as an empty list
        }

        // Method to add a debt (Asynchronous)
        public async Task AddDebt(Depts debt)
        {
            await _deptService.InsertDebt(debt); // Call async method in service
        }

        // Method to retrieve all debts (Asynchronous)
        public async Task LoadAllDebts()
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    db.CreateTable<Depts>();
                    AllDebts = await Task.Run(() => db.Table<Depts>().ToList());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading debts: {ex.Message}");
                    throw;
                }
            }
        }

        // Method to update debt status
        public async Task UpdateDebtStatus(int debtId, Depts.DebtStatus newStatus)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    var debtToUpdate = db.Find<Depts>(debtId);
                    if (debtToUpdate != null)
                    {
                        debtToUpdate.Status = newStatus;
                        await Task.Run(() => db.Update(debtToUpdate));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error updating debt status: {ex.Message}");
                    throw;
                }
            }
        }

        // Method to delete a debt (Asynchronous)
        public async Task DeleteDebt(int debtId)
        {
            using (var db = DataConfig.GetDatabaseConnection())
            {
                try
                {
                    var debtToDelete = db.Find<Depts>(debtId);
                    if (debtToDelete != null)
                    {
                        await Task.Run(() => db.Delete(debtToDelete));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deleting debt: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
