using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class Account
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        
    }
}