using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Personal_Expense_Tracking.Models
{
    internal class Depts
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Source { get; set; }

        [NotNull]
        public decimal Amount { get; set; }

        [NotNull]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [NotNull]
        public DebtStatus Status { get; set; } = DebtStatus.Pending;

        public Depts() { }


        public enum DebtStatus
        {
            Pending,
            Cleared
        }
    }

}
