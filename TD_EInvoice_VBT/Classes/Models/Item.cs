using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Item
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string BuyersItemIdentification { get; set; }
        public string SellersItemIdentification { get; set; }
        public string ManufacturersItemIdentification { get; set; }
        public Id AdditionalItemIdentification { get; set; }
        public Country OriginCountry { get; set; }
        public ItemClassification CommodityClassification { get; set; }
        public ItemInstance ItemInstance { get; set; }
    }
}