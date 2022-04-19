using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Attachment
    {
        public BinaryObject EmbeddedDocumentBinaryObject { get; set; }
        public string ExternalReference { get; set;}
    }
}