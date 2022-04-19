using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class ItemInstance
    {
        public string ProductTraceId { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ManufactureTime { get; set; }
        public DateTime BestBeforeDate { get; set; }
        public string RegistrationId { get; set; }
        public string SerialId { get; set; }
        public ItemProperty AdditionalItemProperty { get; set; }
        public LotIdentification LotIdentification { get; set; }
    }
}