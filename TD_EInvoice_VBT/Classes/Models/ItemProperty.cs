using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class ItemProperty
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameCode { get; set; }
        public string TestMethod { get; set; }
        public string Value { get; set; }
        public Quantity ValueQuantity { get; set; }
        public string ValueQualifier { get; set; }
        public string ImportanceCode { get; set; }
        public string ListValue { get; set; }
        public Period UsabilityPeriod { get; set; }
        public ItemProperty ItemPropertyGroup { get; set; }
        public Dimension RangeDimension { get; set; }
        public ItemPropertyRange ItemPropertyRange { get; set; }

    }
}