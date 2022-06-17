using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Data
    {
        public string InvoiceNumber { get; set; }
        public string Ettn { get; set; }
        public string PdfUrl { get; set; }
        public string XmlUrl { get; set; }
        public bool HasError { get; set; }
        public List<Errors> Errors { get; set; }
    }
}