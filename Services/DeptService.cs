using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Expense_Tracking.Models;
using Personal_Expense_Tracking.Data;
using SQLite;

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
                    await Task.Run(() => db.Insert(debt));
                }
                catch (SQLiteException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error inserting debt: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
