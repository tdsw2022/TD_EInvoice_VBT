using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Stowage
    {
        public string LocationId { get; set; }
        public Location Location { get; set; }
        public Dimension MeasurementDimension { get; set; }
    }
}