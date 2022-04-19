using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class LotIdentification
    {
        public string LotNumberId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ItemProperty AdditionalItemProperty { get; set; }
    }
}