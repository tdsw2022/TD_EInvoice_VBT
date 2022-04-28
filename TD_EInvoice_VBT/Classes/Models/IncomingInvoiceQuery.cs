using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class IncomingInvoiceQuery
    {
        public DateRange IssueDate { get; set; }

        /*public DateRange EnvelopeDate { get; set; }
        public string EnvelopeId { get; set; }
        public string UUId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PartyName { get; set; }
        public string PartyVknTckn { get; set; }
        public string IncomingInvoiceStatusForUser { get; set; }*/
        public Boolean IsErpProcessed { get; set; }
    }
}