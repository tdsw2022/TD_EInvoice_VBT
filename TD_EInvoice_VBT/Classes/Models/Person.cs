using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Title { get; set; }
        public string MiddleName { get; set; }
        public string NameSuffix { get; set; }
        public string NationalityId { get; set; }
        public FinancialAccount FinancialAccount { get; set; }
        public DocumentReference IdentityDocumentReference { get; set; }
        public string IdentityOrTaxNumber { get; set; }
    }
}