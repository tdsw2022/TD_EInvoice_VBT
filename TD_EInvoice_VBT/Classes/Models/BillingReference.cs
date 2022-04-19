using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class BillingReference
    {
        public DocumentReference InvoiceDocumentReference { get; set; }
        public DocumentReference SelfBilledInvoiceDocumentReference { get; set; }
        public DocumentReference CreditNoteDocumentReference { get; set; }
        public DocumentReference SelfBilledCreditNoteDocumentReference { get; set; }
        public DocumentReference DebitNoteDocumentReference { get; set; }
        public DocumentReference ReminderDocumentReference { get; set; }
        public DocumentReference AdditionalDocumentReference { get; set; }
        public BillingReferenceLine BillingReferenceLine { get; set; }
        public Boolean IsUsedAsDespatchAdvice { get; set; }
    }
}