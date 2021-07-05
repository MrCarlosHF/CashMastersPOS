namespace CashMastersPOSCore.Models
{
    public class Cash
    {
        /// <summary>
        /// Denomination of bill or coin
        /// </summary>
        public string Denomination { get; set; }
        /// <summary>
        /// Value of the bill or coin.
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Number of bills or coins.
        /// </summary>
        public int Quantity { get; set; }
    }
}
