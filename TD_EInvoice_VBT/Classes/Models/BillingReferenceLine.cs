using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class BillingReferenceLine
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public AllowanceCharge AllowanceCharge { get; set; }
    }
}