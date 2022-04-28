using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class IncomingInvoiceDataResponse
    {
        public int Total { get; set; }
        public List<IncomingInvoice> Results { get; set; }
    }
}