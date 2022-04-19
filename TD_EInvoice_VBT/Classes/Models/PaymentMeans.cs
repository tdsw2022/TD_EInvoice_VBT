using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class PaymentMeans
    {
        public string PaymentMeansCode { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string PaymentChannelCode { get; set; }
        public string InstructionNote { get; set; }
        public FinancialAccount PayerFinancialAccount { get; set; }
        public FinancialAccount PayeeFinancialAccount { get; set; }
    }
}