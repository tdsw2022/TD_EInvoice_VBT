using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Dimension
    {
        public string AttributeId { get; set; }
        public Measure Measure { get; set; }
        public string Description { get; set; }
        public Measure MinimumMeasure { get; set; }
        public Measure MaximumMeasure { get; set; }
    }
}