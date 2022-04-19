using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Delivery
    {
        public string Id { get; set; }
        public Quantity Quantity { get; set; }
        public DateTime ActualDeliveryDate { get; set; }
        public DateTime ActualDeliveryTime { get; set; }
        public DateTime LatestDeliveryDate { get; set; }
        public DateTime LatestDeliveryTime { get; set; }
        public string TrackingId { get; set; }
        public Address DeliveryAddress { get; set; }
        public Location AlternativeDeliveryLocation { get; set; }
        public Period EstimatedDeliveryPeriod { get; set; }
        public Party CarrierParty { get; set; }
        public Party DeliveryParty { get; set; }
        public Despatch Despatch { get; set; }
        public DeliveryTerms DeliveryTerms { get; set; }
        public Shipment Shipment { get; set; }
    }
}