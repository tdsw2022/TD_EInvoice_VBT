using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class DocumentReference
    {
        public string Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string DocumentTypeCode {get;set;}
        public string DocumentType { get; set; }
        public string[] DocumentDescription { get; set; }
        public Attachment Attachment { get; set; }
        public Period ValidityPeriod { get; set; }
        public Party IssuerParty { get; set; }
    }
}