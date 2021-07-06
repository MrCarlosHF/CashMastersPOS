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

        /// <summary>
        /// Configurable currency
        /// </summary>
        private Dictionary<string, decimal> CurrentCurrencyDictionary => SetCurrency();

        public CashMastersService(ILogger<CashMastersService> log,
            IConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;
        }

        /// <summary>
        /// Calculates the correct change and returns the optimum(i.e.minimum) number of bills and coins.
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public Change CashChange(Payment payment)
        {
            var cashChangeResult = new Change();

            try
            {
                if (payment == null 
                    || payment.Price < 0 || payment.TotalPaid < 0
                    || payment.TotalPaid < payment.Price)
                {                    
                    _log.LogWarning($"Please, review your payment: Price = {payment?.Price} and total paid = {payment?.TotalPaid}");
                    return null;
                }

                decimal totalChange = payment.TotalPaid - payment.Price;
                decimal remainder = totalChange;
                cashChangeResult.CashChange = InitializeCashListCurrency();
                //Calculates of number of each bills and coins.
                foreach (var cash in cashChangeResult.CashChange)
                {
                    while (remainder >= cash.Value)
                    {
                        cash.Quantity = (int)Math.Truncate(remainder / cash.Value);
                        remainder %= cash.Value;                        
                    }
                }
            }
            catch(Exception ex)
            {
                //File in Logs/console.log
                _log.LogError($"Exception in CashMastersService.CashChange: {ex}");
            }
            
            return cashChangeResult;
        }

        /// <summary>
        /// Initialize the Cash list with the current currency values.
        /// </summary>
        /// <returns></returns>
        public List<Cash> InitializeCashListCurrency()
        {
            var cashList = new List<Cash>();
            foreach (var currencyItem in CurrentCurrencyDictionary)
            {
                cashList.Add(new Cash() { Denomination = currencyItem.Key, Value = currencyItem.Value });
            }

            return cashList;
        }

        /// <summary>
        /// Set the current currency depending on the configuration in the appsettings.
        /// Available currencies: MXN and US.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, decimal> SetCurrency()
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
                currencyDictionary.Add("0.25", 0.25m);
                currencyDictionary.Add("0.10", 0.10m);
                currencyDictionary.Add("0.05", 0.05m);
                currencyDictionary.Add("0.01", 0.01m);
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
                currencyDictionary.Add("0.20", 0.20m);
                currencyDictionary.Add("0.10", 0.10m);
                currencyDictionary.Add("0.05", 0.05m);
            }

            return currencyDictionary;
        }        
    }    
}
