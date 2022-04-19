using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class InternetPayment
    {
        public string WebAdresi { get; set; }
        public string OdemeSekli { get; set; }
        public string OdemeSekliAciklama { get; set; }
        public string OdemeAracisiAdi { get; set; }
        public DateTime OdemeTarihi { get; set; }
        public string GonderiTasiyanVknTckn { get; set; }
        public string GonderiTasiyanUnvan { get; set; }
        public DateTime GonderimTarihi { get; set; }
        public string Gonderilen { get; set; }
    }
}