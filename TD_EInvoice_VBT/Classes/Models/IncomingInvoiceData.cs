using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class IncomingInvoiceData
    {
        public int Total { get; set; }
        public List<IncomingInvoiceResponseDetail> Results { get; set; }
    }
}