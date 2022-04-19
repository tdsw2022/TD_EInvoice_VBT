using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class SupplierParty
    {
        public Party Party { get; set; }
        public Despatch DespatcContact { get; set; }
    }
}