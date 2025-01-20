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
            try
            {
                await _deptService.InsertDebt(debt); // Call async method in service to insert debt
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding debt: {ex.Message}");
                throw;
            }
        }

        // Method to load all debts from the service (Asynchronous)
        public async Task LoadAllDebts()
        {
            try
            {
                AllDebts = await _deptService.GetAllDebts(); // Fetch all debts using service
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading debts: {ex.Message}");
                throw;
            }
        }

        // Method to update a debt status (Asynchronous)
        public async Task UpdateDebtStatus(int debtId, Depts.DebtStatus newStatus)
        {
            try
            {
                // Get the debt by ID
                var debt = await _deptService.GetDebtById(debtId);
                if (debt != null)
                {
                    debt.Status = newStatus; // Update the status
                    await _deptService.UpdateDebt(debt); // Update the debt in the service
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating debt status: {ex.Message}");
                throw;
            }
        }

        // Method to delete a debt (Asynchronous)
        public async Task DeleteDebt(int debtId)
        {
            try
            {
                // Get the debt by ID
                var debt = await _deptService.GetDebtById(debtId);
                if (debt != null)
                {
                    await _deptService.DeleteDebt(debt); // Delete the debt using the service
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting debt: {ex.Message}");
                throw;
            }
        }

        // Method to get the total debt amount (Asynchronous)
        public async Task<decimal> GetTotalDebtAmount()
        {
            try
            {
                return await _deptService.GetTotalDeptAmount(); // Call service to get the total debt amount
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating total debt amount: {ex.Message}");
                throw;
            }
        }
        public async Task<decimal> GetTotalPendingAmount(decimal creditAmount)
        {
            try
            {
                // Call the service to get the total pending debt amount
                return await _deptService.GetTotalPendingDeptAmount(creditAmount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating total pending amount: {ex.Message}");
                throw;
            }

        }
      

       

    }
}
