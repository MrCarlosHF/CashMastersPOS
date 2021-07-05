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
        public decimal PaidCash { get; private set; }

        public Payment(decimal price, decimal paidCash)
        {
            Price = price;
            PaidCash = paidCash;
        }
    }
}