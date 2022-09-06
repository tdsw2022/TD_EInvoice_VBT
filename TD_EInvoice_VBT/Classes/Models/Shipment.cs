using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Shipment
    {
        public Id Id { get; set; }
        public string HandlingCode { get; set; }
        public string HandlingInstructions { get; set; }
        public Measure GrossWeightMeasure { get; set; }
        public Measure NetWeightMeasure { get; set; }
        public Measure GrossVolumeMeasure { get; set; }
        public Measure NetVolumeMeasure { get; set; }
        public Quantity TotalGoodsItemQuantity { get; set; }
        public Quantity TotalTransportHandlingUnitQuantity { get; set; }
        public double InsuranceValueAmount { get; set; }
        public double DeclaredCustomsValueAmount { get; set; }
        public double DeclaredForCarriageValueAmount { get; set; }
        public double DeclaredStatisticsValueAmount { get; set; }
        public double FreeOnBoardValueAmount { get; set; }
        public string SpecialInstructions { get; set; }
        public List<GoodsItem> GoodsItem { get; set; }
        public List<ShipmentStage>ShipmentStage { get; set; }
        public Delivery Delivery { get; set; }
        public List<TransportHandlingUnit> TransportHandlingUnit { get; set; }
        public Address ReturnAddress { get; set; }
        public Location FirstArrivalPortLocation { get; set; }
        public Location LastExitPortLocation { get; set; }

    }
}