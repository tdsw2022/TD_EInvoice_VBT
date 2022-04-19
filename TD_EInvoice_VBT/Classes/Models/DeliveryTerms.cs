using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class DeliveryTerms
    {
        public Id Id { get; set; }
        public string SpecialTerms { get; set; }
        public double Amount { get; set; }
    }
}