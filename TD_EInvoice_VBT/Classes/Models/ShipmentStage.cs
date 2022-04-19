using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class ShipmentStage
    {
        public string Id { get; set; }
        public string TransportModeCode { get; set; }
        public string TransportMeansTypeCode { get; set; }
        public string TransitDirectionCode { get; set; }
        public string Instructions { get; set; }
        public Period TransitPeriod { get; set; }
        public TransportMeans TransportMeans { get; set; }
        public Person DriverPerson { get; set; }
    }
}