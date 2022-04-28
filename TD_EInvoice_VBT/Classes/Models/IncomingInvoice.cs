using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class IncomingInvoice
    {
        public IncomingInvoiceQuery Query { get; set; }
        //public Int32 Skip { get; set; }
        public Int32 Take { get; set; }
        //public string OrderByName { get; set; }
        //public string OrderByType { get; set; }
    }
}