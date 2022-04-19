using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class MaritimeTransport
    {
        public Id VesselId { get; set; }
        public string VesselName { get; set; }
        public string RadioCallSignId { get; set; }
        public string ShipsRequirements { get; set; }
        public Measure GrossTonnageMeasure { get; set; }
        public Measure NetTonnageMeasure { get; set; }
        public DocumentReference RegistryCertificateDocumentReference { get; set; }
        public Location RegistryPortLocation { get; set; }
    }
}