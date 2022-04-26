using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Models
{
    public class UserResponse
    {
        public string RefreshToken { get; set; }
        public UserData Data { get; set; }
    }
}