using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class PaymentTerms
    {
        public string Note { get; set; }
        public double PenaltySurchargePercent { get; set; }
        public double Amount { get; set; }
        public double PenaltyAmount { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public Period SettlementPeriod { get; set; }
    }
}