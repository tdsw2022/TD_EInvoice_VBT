using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class FinancialAccount
    {
        public string Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyId { get; set; }
        public string PaymentNote { get; set; }
        public Branch FinancialInstitutionBranch { get; set; }
    }
}