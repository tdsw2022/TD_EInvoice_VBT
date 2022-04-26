using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class AliasResponse
    {
        public string Name { get; set; } //Etiket Adı
        public string CreationTime { get; set; } //Etiket oluşturulma tarihi
        public string DeletionTime { get; set; } //Etiket Silme tarihi
    }
}