using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class CorporateRegistration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CorporateRegistrationTypeCode { get; set; }
        public Address JurisdictionRegionAddress { get; set; }

    }
}