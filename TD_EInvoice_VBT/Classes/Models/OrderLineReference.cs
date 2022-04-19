using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class OrderLineReference
    {
        public string LineId { get; set; }
        public string SalesOrderLineId { get; set; }
        public string UUId { get; set; }
        public string LineStatusCode { get; set; }
        public OrderReference OrderReference { get; set; }
    }
}