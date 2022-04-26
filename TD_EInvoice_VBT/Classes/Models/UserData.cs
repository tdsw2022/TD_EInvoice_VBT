using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class UserData
    {
        public string Identifier { get; set; } //VKN
        public string Title { get; set; } //Mükellef Adı
        public string Type { get; set; } //Özel veya Kamu tipi
        public DateTime FirstCreationTime { get; set; }//E-Faturaya geçiş tarihi
        public string AccountType { get; set; }//Faturadan yararlanma tipi
        public List<DocumentResponse> Documents { get; set; }
    }
}