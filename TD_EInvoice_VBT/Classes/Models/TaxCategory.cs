using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class TaxCategory
    {
        public string Name { get; set; }
        public string TaxExemptionReasonCode { get; set; }
        public string TaxExemptionReason { get; set; }
        public TaxScheme TaxScheme { get; set; }
    }
}