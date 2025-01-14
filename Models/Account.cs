using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Personal_Expense_Tracking.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public decimal Balance { get; set; } = 0;
        public decimal TotalCredit { get; set; } =0;
        public decimal TotalDebit { get; set; } = 0;
        public decimal TotalDebt { get; set; } = 0;

    }
}
