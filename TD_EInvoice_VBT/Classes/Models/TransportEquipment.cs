using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class TransportEquipment
    {
        public Id Id { get; set; }
        public string TransportEquipmentTypeCode { get; set; }
        public string Description { get; set; }
    }
}