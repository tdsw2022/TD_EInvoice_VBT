using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class HazardousGoods
    {
        public string TransportEmergencyCardCode { get; set; }
        public string PackingCriteriaCode { get; set; }
        public string HazardousRegulationCode { get; set; }
        public string InhalationToxicityZoneCode { get; set; }
        public string TransportAuthorizationCode { get; set; }
        public Temperature MaximumTemperature { get; set; }
        public Temperature MinimumTemperature { get; set; }
    }
}