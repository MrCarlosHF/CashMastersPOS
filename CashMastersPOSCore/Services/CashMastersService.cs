using CashMastersPOSCore.Interfaces;
using CashMastersPOSCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CashMastersPOSCore.Services
{
    public class CashMastersService : ICashMastersService
    {
        private readonly ILogger<CashMastersService> _log;
        private readonly IConfiguration _configuration;                         
        private List<Cash> cashChange;
        /// <summary>
        /// Configurable currency
        /// </summary>
        private Dictionary<string, decimal> CurrentCurrency => GetCurrency();

        public CashMastersService(ILogger<CashMastersService> log,
            IConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;

            cashChange = new List<Cash>();
            foreach (var currencyItem in CurrentCurrency)
            {
                cashChange.Add(new Cash() { Denomination = currencyItem.Key, Value = currencyItem.Value });
            }
        }

        /// <summary>
        /// Calculates the correct change and returns the optimum(i.e.minimum) number of bills and coins.
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public Change CashChange(Payment payment)
        {
            Change cashChangeResult = null;

            try
            {
                if (payment == null || payment.PaidCash < payment.Price)
                {                    
                    _log.LogWarning("Please, review your payment.");
                    return cashChangeResult;
                }

                decimal totalChange = payment.PaidCash - payment.Price;
                decimal remainder = totalChange;
                //Calculates of number of each bills and coins.
                foreach (var cash in cashChange)
                {
                    while (remainder >= cash.Value)
                    {
                        cash.Quantity = (int)Math.Truncate(remainder / cash.Value);
                        remainder %= cash.Value;                        
                    }
                }

                cashChangeResult = new Change()
                {
                    CashChange = cashChange
                };
            }
            catch(Exception ex)
            {
                //File in Logs/console.log
                _log.LogError($"Exception in CashMastersService.CashChange: {ex}");
            }
            
            return cashChangeResult;
        }

        /// <summary>
        /// Gets the current currency depending on the configuration in the appsettings.
        /// Available currencies: MXN and US.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, decimal> GetCurrency()
        {
            string currency = _configuration["Currency"];
            var currencyDictionary = new Dictionary<string, decimal>();
            if (!string.IsNullOrEmpty(currency) && currency == "US")
            {
                currencyDictionary.Add("100.00", 100.00m);
                currencyDictionary.Add("50.00", 50.00m);
                currencyDictionary.Add("20.00", 20.00m);
                currencyDictionary.Add("10.00", 10.00m);
                currencyDictionary.Add("5.00", 5.00m);
                currencyDictionary.Add("2.00", 2.00m);
                currencyDictionary.Add("1.00", 1.00m);
                currencyDictionary.Add("0.50", 0.50m);
                currencyDictionary.Add("0.20", 0.20m);
                currencyDictionary.Add("0.10", 0.10m);
                currencyDictionary.Add("0.05", 0.05m);
            }
            //Default currency == "MXN"
            else
            {
                currencyDictionary.Add("100.00", 100.00m);
                currencyDictionary.Add("50.00", 50.00m);
                currencyDictionary.Add("20.00", 20.00m);
                currencyDictionary.Add("10.00", 10.00m);
                currencyDictionary.Add("5.00", 5.00m);
                currencyDictionary.Add("2.00", 2.00m);
                currencyDictionary.Add("1.00", 1.00m);
                currencyDictionary.Add("0.50", 0.50m);
                currencyDictionary.Add("0.25", 0.25m);
                currencyDictionary.Add("0.10", 0.10m);
                currencyDictionary.Add("0.05", 0.05m);
                currencyDictionary.Add("0.01", 0.05m);
            }

            return currencyDictionary;
        }
    }    
}
