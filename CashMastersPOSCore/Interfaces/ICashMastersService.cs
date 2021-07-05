using CashMastersPOSCore.Models;
using System.Collections.Generic;

namespace CashMastersPOSCore.Interfaces
{
    public interface ICashMastersService
    {
        Change CashChange(Payment payment);
    }
}
