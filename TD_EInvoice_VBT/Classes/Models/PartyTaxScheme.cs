using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class PartyTaxScheme
    {
        public TaxScheme TaxScheme { get; set; }
        public string RegistrationName { get; set; }
        public string CompanyId { get; set; }
    }
}