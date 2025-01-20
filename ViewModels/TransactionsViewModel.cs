using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Personal_Expense_Tracking.Services;
using Personal_Expense_Tracking.Models;
using Microsoft.AspNetCore.Components;

namespace Personal_Expense_Tracking.ViewModels
{
    public class TransactionsViewModel
    {
        private TransactionsService _transactionsService;
        private DeptService _deptsService; // Declare DeptsService

        // This property will hold all transactions
        public List<Transactions> AllTransactions { get; set; }

        // Constructor to initialize services
        public TransactionsViewModel()
        {
            _transactionsService = new TransactionsService(); // Initialize TransactionsService
            _deptsService = new DeptService(); // Initialize DeptsService
            AllTransactions = new List<Transactions>(); // Initialize as an empty list
        }

        // Method to add a transaction (Asynchronous)
        public async Task AddTransaction(Transactions transaction)
        {
            await _transactionsService.InsertTransaction(transaction); // Call async method in service
        }

        // Method to delete a transaction (Asynchronous)
        public async Task DeleteTransaction(int transactionId)
        {
            // Call the async DeleteTransaction method from the service
            await _transactionsService.DeleteTransaction(transactionId);

            // After deleting, refresh the list of transactions to reflect the changes
            AllTransactions = await _transactionsService.GetAllTransactions(); // Get updated transactions list asynchronously
        }

        // Method to fetch all transactions (Asynchronous)
        public async Task<List<Transactions>> GetTransactions()
        {
            AllTransactions = await _transactionsService.GetAllTransactions();
            return AllTransactions;
        }

        // Method to get total inflow
        public async Task<decimal> GetTotalInflow()
        {
            return await _transactionsService.GetTotalInflow(); // Fetch total inflow from the service
        }

        // Method to get total outflow
        public async Task<decimal> GetTotalOutflow()
        {
            return await _transactionsService.GetTotalOutflow(); // Fetch total outflow from the service
        }



        // Method to get total numbers of transaction
        public async Task<int> GetTotalNumberOfTransactions()
        {
            return await _transactionsService.GetTotalNumberOfTransactions(); // Fetch total number of transaction from the service
        }

        public async Task<decimal> GetTotalBalance()
        {
            try
            {
                // Fetch inflows and outflows
                decimal totalCredit = await GetTotalInflow();
                decimal totalDebit = await GetTotalOutflow();

                // Fetch pending debts
                decimal pendingDebt = await _deptsService.GetTotalPendingDeptAmount(totalCredit);

                // Calculate the total balance
                decimal totalBalance = totalCredit - totalDebit + pendingDebt;

                return totalBalance;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating total balance: {ex.Message}");
                throw;
            }
        }
    }
}
