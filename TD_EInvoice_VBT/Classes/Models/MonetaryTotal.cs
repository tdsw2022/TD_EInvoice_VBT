using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class MonetaryTotal
    {
        public double LineExtensionAmount { get; set; }
        public double TaxExclusiveAmount { get; set; }
        public double TaxInclusiveAmount { get; set; }
        public double AllowanceTotalAmount { get; set; }
        public double ChargeTotalAmount { get; set; }
        public double PayableRoundingAmount { get; set; }
        public double PayableAmount { get; set; }
    }
}