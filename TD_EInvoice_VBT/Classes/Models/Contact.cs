using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Telefax { get; set; }
        public string ElectronicMail { get; set; }
        public string Note { get; set; }
        public Communication OtherCommunication { get; set; }

    }
}