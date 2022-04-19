using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class InvoiceLine
    {
        public string Id { get; set; }
        public string InvoiceLineExternalId { get; set; }
        public string Note { get; set; }
        public Quantity InvoicedQuantity { get; set; }
        public double LineExtensionAmount { get; set; }
        public AllowanceCharge AllowanceCharge { get; set; }
        public TaxTotal TaxTotal { get; set; }
        public TaxTotal WithholdingTaxTotal { get; set; }
        public Item Item { get; set; }
        public double Price { get; set; }
        public OrderLineReference OrderLineReference { get; set; }
        public LineReference DespatchLineReference { get; set; }
        public LineReference ReceiptLineReference { get; set; }
        public Delivery Delivery { get; set; }
        public InvoiceLine SubInvoiceLine { get; set; }
    }
}