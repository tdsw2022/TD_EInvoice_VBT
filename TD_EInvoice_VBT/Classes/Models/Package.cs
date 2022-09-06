using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Package
    {
        public string Id { get; set; }
        public Quantity Quantity { get; set; }
        public Boolean ReturnableMaterialIndicator { get; set; }
        public string PackageLevelCode { get; set; }
        public string PackaingTypeCode { get; set; }
        public string PackingMaterial { get; set; }
        public Package ContainedPackage { get; set; }
        public GoodsItem GoodsItem { get; set; }
        public Dimension MeasurementDimension { get; set; }
    }
}