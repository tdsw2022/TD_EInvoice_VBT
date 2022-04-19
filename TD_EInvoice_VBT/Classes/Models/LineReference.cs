using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class LineReference
    {
        public string LineId { get; set; }
        public string LineStatusCode { get; set; }
        public DocumentReference DocumentReference { get; set; }
    }
}