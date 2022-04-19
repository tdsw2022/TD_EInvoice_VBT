using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Party
    {
        public Int32 Id { get; set; }
        public string PartyExternalId { get; set; }
        public string WebsiteURI { get; set; }
        public List<Id> PartyIdentifications { get; set; }
        public string PartyName { get; set; }
        public Address PostalAddress { get; set; }
        public Contact Contact { get; set; }
        public PartyTaxScheme PartyTaxScheme { get; set; }
        public Person Person { get; set; }
        public string EndpointId { get; set; }
        public string IndustryClassificationCode { get; set; }
        public Location PhysicalLocation { get; set; }
        public PartyLegalEntity PartyLegalEntity { get; set; }
        public Party AgentParty { get; set; }
        public DateTime OpenningBalanceDate { get; set; }
        public double OpenningBalance { get; set; }
        public Int32 CategoryId { get; set; }
        public string IdentityOrTaxNumber { get; set; }
        public string LegalOrPerson { get; set; }
        public string EArchiveMailTo { get; set; }

    }
}