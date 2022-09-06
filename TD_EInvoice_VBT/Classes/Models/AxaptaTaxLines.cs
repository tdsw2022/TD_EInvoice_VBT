using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class AxaptaTaxLines
    {
        public Int64 custInvoiceTransRecId { get; set; }

        public string taxRecId { get; set; }

        public string eInvoiceRecId { get; set; }

        public string taxValue { get; set; }

        public double sourceTaxAmountCur { get; set; }
    }
}