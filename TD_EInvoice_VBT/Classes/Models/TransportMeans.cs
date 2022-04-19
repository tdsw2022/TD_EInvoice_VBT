using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class TransportMeans
    {
        public string JourneyId { get; set; }
        public string RegistrationNationalityId { get; set; }
        public string RegistrationNationality { get; set; }
        public string DirectionCode { get; set; }
        public string TransportMeansTypeCode { get; set; }
        public string TradeServiceCode { get; set; }
        public Stowage Stowage { get; set; }
        public AirTransport AirTransport { get; set; }
        public RoadTransport RoadTransport { get; set; }
        public RailTransport RailTransport { get; set; }
        public MaritimeTransport MaritimeTransport { get; set; }
        public Party OwnerParty { get; set; }
        public Dimension MeasurementDimension { get; set; }

    }
}