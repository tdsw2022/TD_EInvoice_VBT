using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class IncomingInvoiceResponse
    {
        public string Type{ get; set; }
        public string Type_Desc{ get; set; }
        public string ErrorCode{ get; set; }
        public string Message { get; set; }
        public IncomingInvoiceResponseDetail Detail { get; set; }
    }
}