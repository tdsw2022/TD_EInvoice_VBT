using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class BinaryObject
    {
        public string Format { get; set; }
        public string MimeCode { get; set; }
        public string EncodingCode { get; set; }
        public string CharacterSetCode { get; set; }
        public string Uri { get; set; }
        public string Filename { get; set; }
        public string[] Value { get; set; }
    }
}