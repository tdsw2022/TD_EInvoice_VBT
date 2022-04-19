using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class OutgoingInvoice
    {
        public string LocationCode { get; set; }
        public string XsltFileName { get; set; }
        public string FirmBranchCode { get; set; }
        public string TryCountDescription { get; set; }
        public Int32 TryCount { get; set; }
        public string EArchiveMailTo { get; set; }
        public Boolean PutInvoiceNumberIntoQrCode { get; set; }
        public string MailTo { get; set; }
        public string ReceiverIdentifier { get; set; }
        public string ChannelCode { get; set; }
        public Int32 PartyGroupId { get; set; }
        public string PartyGroupName { get; set; }
        public Boolean IsOocumentNumberIncluded { get; set; }
        public Barcode Barcode { get; set; }
        public Int32 Id { get; set; }
        public string InvoiceExternalId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ProfileId { get; set; }
        public string UUId { get; set; }
        public string InvoiceTypeCode { get; set; }
        public string DocumentCurrencyCode { get; set; }
        public Boolean CopyIndicator { get; set; }
        public DateTime IssueDate { get; set; }
        public double LineCountNumeric { get; set; }
        public List<string> Note { get; set; }
        public SupplierParty AccountingSupplierParty { get; set; }
        public CustomerParty AccountingCustomerParty { get; set; }
        public List<AllowanceCharge> AllowanceCharge { get; set; }
        public List<TaxTotal> TaxTotal { get; set; }
        public List<TaxTotal> WithholdingTaxTotal { get; set; }
        public MonetaryTotal LegalMonetaryTotal { get; set; }
        public List<InvoiceLine> InvoiceLine { get; set; }
        public string AccountingCost { get; set; }
        public string TaxCurrencyCode { get; set; }
        public string PricingCurrencyCode { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string PaymentAlternativeCurrencyCode { get; set; }
        public Period InvoicePeriod { get; set; }
        public OrderReference OrderReference { get; set; }
        public List<BillingReference> BillingReference { get; set; }
        public DocumentReference DespatchDocumentReference { get; set; }
        public DocumentReference ReceiptDocumentReference { get; set; }
        public DocumentReference OriginatorDocumentReference { get; set; }
        public DocumentReference ContractDocumentReference { get; set; }
        public DocumentReference AdditionalDocumentReference { get; set; }
        public CustomerParty BuyerCustomerParty { get; set; }
        public SupplierParty SellerSupplierParty { get; set; }
        public Party TaxRepresentativeParty { get; set; }
        public Delivery Delivery { get; set; }
        public PaymentMeans PaymentMeans { get; set; }
        public PaymentTerms PaymentTerms { get; set; }
        public ExchangeRate TaxExchangeRate { get; set; }
        public ExchangeRate PricingExchangeRate { get; set; }
        public ExchangeRate PaymentExchangeRate { get; set; }
        public ExchangeRate PaymentAlternativeExchangeRate { get; set; }
        public InternetPayment InternetPayment { get; set; }
        public DateTime DatePosted { get; set; }

    }
}