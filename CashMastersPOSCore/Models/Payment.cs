using System;
using System.Collections.Generic;
using System.Linq;

namespace CashMastersPOSCore.Models
{
    public class Payment
    {
        /// <summary>
        /// Purchased Item Price
        /// </summary>
        public decimal Price { get; private set; }
        /// <summary>
        /// Paid cash
        /// </summary>
        public List<Cash> PaidCash { get; private set; }

        /// <summary>
        /// Total paid.
        /// </summary>
        public decimal TotalPaid => CalculateTotalPaid();

        public Payment(decimal price, List<Cash> paidCash)
        {
            Price = price;
            PaidCash = paidCash;
        }

        /// <summary>
        /// Calculates the total change based on the PaidCash.
        /// </summary>
        /// <returns></returns>
        private decimal CalculateTotalPaid()
        {
            decimal total = PaidCash?.Sum(c => c.Quantity * c.Value) ?? 0.00m;

            return total;
        }

    }
}