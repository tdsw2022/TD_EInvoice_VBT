using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class GoodsItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Boolean HazardousRiskIndicator { get; set; }
        public double DeclaredCustomsValueAmount { get; set; }
        public double DeclaredForCarriageValueAmount { get; set; }
        public double DeclaredStatisticsValueAmount { get; set; }
        public double FreeOnBoardValueAmount { get; set; }
        public double InsuranceValueAmount { get; set; }
        public double ValueAmount { get; set; }
        public Measure GrossWeightMeasure { get; set; }
        public Measure NetWeightMeasure { get; set; }
        public Measure ChargeableWeightMeasure { get; set; }
        public Measure GrossVolumeMeasure { get; set; }
        public Measure NetVolumeMeasure { get; set; }
        public Quantity Quantity { get; set; }
        public string RequiredCustomsId { get; set; }
        public string CustomsStatusCode { get; set; }
        public Quantity CustomsTariffQuantity { get; set; }
        public Boolean CustomsImportClassifiedIndicator { get; set; }
        public Quantity ChargeableQuantity { get; set; }
        public Quantity ReturnableQuantity { get; set; }
        public string TraceId { get; set; }
        public Item Item { get; set; }
        public AllowanceCharge FreightAllowanceCharge { get; set; }
        public InvoiceLine InvoiceLine { get; set; }
        public Temperature Temperature { get; set; }
        public Address OriginAddress { get; set; }
        public Dimension MeasurementDimension { get; set; }
    }
}