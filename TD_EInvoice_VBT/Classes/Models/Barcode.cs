using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Barcode
    {
        public string BarcodeName { get; set; }
        public string BarcodeFormat { get; set; }
        public string CharSet { get; set; }
        public string[] Content { get; set; }

    }
}