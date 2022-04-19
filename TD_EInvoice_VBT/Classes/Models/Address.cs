using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Address
    {
        public string Id { get; set; }
        public string Postbox { get; set; }
        public string Room { get; set; }
        public string StreetName { get; set; }
        public string BlockName { get; set; }
        public string BuildingName { get; set; }
        public string BuildingNumber { get; set; }
        public string CitySubdivisionName { get; set; }
        public string CityName { get; set; }
        public string PostalZone { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public Country Country { get; set; }
    }
}