using Microsoft.VisualStudio.TestTools.UnitTesting;
using CashMastersPOSCore.Services;
using Microsoft.Extensions.DependencyInjection;
using CashMastersPOSCore.Interfaces;
using CashMastersPOSTest;
using CashMastersPOSCore.Models;
using System.Collections.Generic;

namespace CashMastersPOSCoreTest
{
    [TestClass]
    public class CashMastersServiceTest
    {
        private readonly ICashMastersService service;        

        public CashMastersServiceTest()
        {
            var startup = new Startup();
            service = startup.ServiceProvider.GetRequiredService<ICashMastersService>();
        }

        [TestMethod]
        public void CashChange_ValidMXNPayment_ShouldReturnChange()
        {
            //Arrange
            decimal price = 105.50m;
            decimal expectedChange = 4.50m;
            List<Cash> paidCash = service.InitializeCashListCurrency();
            paidCash.Find(c => c.Denomination == "100.00").Quantity = 1;
            paidCash.Find(c => c.Denomination == "5.00").Quantity = 2;

            // Act 
            Change change = service.CashChange(new Payment(price, paidCash));

            //Assert
            Assert.IsNotNull(change);
            Assert.IsTrue(change.CashChange.Count > 0);
            Assert.AreEqual(expectedChange, change.TotalChange);
        }

        [TestMethod]
        public void CashChange_InvalidMXNPayment_ShouldReturnNull()
        {
            //Arrange
            decimal price = 250.05m;
            List<Cash> paidCash = service.InitializeCashListCurrency();            
            paidCash.Find(c => c.Denomination == "10.00").Quantity = 20;

            // Act 
            Change change = service.CashChange(new Payment(price, paidCash));

            //Assert
            Assert.IsNull(change);
        }

    }
}
