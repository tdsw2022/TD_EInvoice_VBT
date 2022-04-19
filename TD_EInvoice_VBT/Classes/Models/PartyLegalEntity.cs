using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class PartyLegalEntity
    {
        public string RegistrationName { get; set; }
        public string CompanyId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Boolean SoleProprietorshipIndicator { get; set; }
        public double CorporateStockAmount { get; set; }
        public Boolean FullyPaidSharesIndicator { get; set; }
        public CorporateRegistration CorporateRegistrationScheme { get; set; }
        public Party HeadOfficeParty { get; set; }
    }
}