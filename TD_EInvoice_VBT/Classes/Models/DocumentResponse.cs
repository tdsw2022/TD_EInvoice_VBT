using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class DocumentResponse
    {
        public List<AliasResponse> Aliases { get; set; }
        public string Type { get; set; } //Etiket Tipi

    }
}