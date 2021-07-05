using CashMastersPOSCore.Interfaces;
using CashMastersPOSCore.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CashMastersPOS
{
    
    public class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var service = startup.ServiceProvider.GetRequiredService<ICashMastersService>();
            try
            {
                Console.Write("Item Price? ");
                decimal price = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Paid cash? ");
                decimal paid = Convert.ToDecimal(Console.ReadLine());

                Change change = service.CashChange(new Payment(price, paid));
                DisplayCashChange(change);
            }
            catch
            {
                Console.Write("Something went wrong, try again.");
            }
            
        }

        /// <summary>
        /// Displays the cash change.
        /// </summary>
        /// <param name="cashChange"></param>
        private static void DisplayCashChange(Change change)
        {
            if (change == null || change.CashChange?.Count == 0)
            {                                
                return;
            }

            Console.Write($"Total change: {change.TotalChange}\n\r");
            foreach (var cash in change.CashChange)
            {
                Console.Write($"{cash.Denomination} x {cash.Quantity}\n\r");
            }            
        }
    }
}
