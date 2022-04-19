using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class AllowanceCharge
    {
        public Boolean ChargeIndicator { get; set; }
        public string AllowanceChargeReason { get; set; }
        public double MultiplierFactorNumeric { get; set; }
        public double SequenceNumeric { get; set; }
        public double Amount { get; set; }
        public double BaseAmount { get; set; }
        public double PerUnitAmount { get; set; }
    }
}