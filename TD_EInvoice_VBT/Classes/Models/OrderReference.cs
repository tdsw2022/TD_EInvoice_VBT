using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class OrderReference
    {
        public string Id { get; set; }
        public string SalesOrderId { get; set; }
        public DateTime IssueDate { get; set; }
        public string OrderTypeCode { get; set; }
        public DocumentReference DocumentReference { get; set; }
    }
}