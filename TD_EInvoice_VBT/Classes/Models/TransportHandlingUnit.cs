using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class TransportHandlingUnit
    {
        public string Id { get; set; }
        public string TransportHandlingUnitTypeCode { get; set; }
        public string HandlingCode { get; set; }
        public string HandlingInstructions { get; set; }
        public Boolean HazardousRiskIndicator { get; set; }
        public Quantity TotalGoodsItemQuantity { get; set; }
        public Quantity TotalPackageQuantity { get; set; }
        public string DamageRemarks { get; set; }
        public string TraceId { get; set; }
        public Package ActualPackage { get; set; }
        public TransportEquipment TransportEquipment { get; set; }
        public TransportMeans TransportMeans { get; set; }
        public HazardousGoods HazardousGoodsTransit { get; set; }
        public Dimension MeasurementDimension { get; set; }
        public Temperature MinimumTemperature { get; set; }
        public Temperature MaximumTemperature { get; set; }
        public Dimension FloorSpaceMeasurementDimension { get; set; }
        public Dimension PalletSpaceMeasurementDimension { get; set; }
        public DocumentReference ShipmentDocumentReference { get; set; }
        public CustomsDeclaration CustomsDeclaration { get; set; }
    }
}