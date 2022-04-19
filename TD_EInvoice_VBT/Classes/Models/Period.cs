using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Period
    {
        public DateTime StartDate { get; set;}
        public DateTime EndDate { get; set; }
        public Measure DurationMeasure { get; set; }
        public string Description { get; set; }
    }
}