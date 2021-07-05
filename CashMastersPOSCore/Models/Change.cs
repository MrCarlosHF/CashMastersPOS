using System.Collections.Generic;
using System.Linq;

namespace CashMastersPOSCore.Models
{
    public class Change
    {
        /// <summary>
        /// Total amount of change.
        /// </summary>
        public decimal TotalChange => CalculateTotalChange();

        /// <summary>
        /// Cash change (bills and coins).
        /// </summary>
        public List<Cash> CashChange { get; set; }      
        
        /// <summary>
        /// Calculates the total change based on the CashChange.
        /// </summary>
        /// <returns></returns>
        private decimal CalculateTotalChange()
        {
            decimal change = CashChange?.Sum(c => c.Quantity * c.Value) ?? 0.00m;            

            return change;
        }
    }
}
