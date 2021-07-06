using CashMastersPOSCore.Interfaces;
using CashMastersPOSCore.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CashMastersPOS
{
    
    public class Program
    {
        static void Main(string[] args)
        {            
            try
            {
                bool run = true;
                var startup = new Startup();
                var service = startup.ServiceProvider.GetRequiredService<ICashMastersService>();

                while (run)
                {
                    List<Cash> paidCash = service.InitializeCashListCurrency();

                    Console.Write("Item Price? ");
                    decimal price = Convert.ToDecimal(Console.ReadLine());
                    Console.Write($"Quantity of bills or coins paid with denomination of: \n\r");
                    foreach (var cash in paidCash)
                    {
                        Console.Write($"{cash.Denomination} = ");
                        cash.Quantity = Convert.ToInt32(Console.ReadLine());
                    }


                    Change change = service.CashChange(new Payment(price, paidCash));
                    DisplayCashChange(change);

                    Console.Write("Do you like to run it again? Yes[Y], NO[Other].");
                    run = Console.ReadKey().Key == ConsoleKey.Y;
                    Console.Clear();
                }               
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
                if (cash.Quantity > 0)
                {
                    Console.Write($"{cash.Denomination} x {cash.Quantity}\n\r");
                }
            }            
        }
    }
}
