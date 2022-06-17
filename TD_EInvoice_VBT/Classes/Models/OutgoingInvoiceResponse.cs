using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class OutgoingInvoiceResponse
    {
        public string RefreshToken { get; set; }
        public Data Data { get; set; }
    }
}