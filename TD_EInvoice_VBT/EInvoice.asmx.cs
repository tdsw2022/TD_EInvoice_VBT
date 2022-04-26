﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TD_EInvoice_VBT.Classes.Models;
using TD_EInvoice_VBT.Classes.Helper;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Dynamics.BusinessConnectorNet;
using System.Data;

namespace TD_EInvoice_VBT
{
    /// <summary>
    /// Summary description for EInvoice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EInvoice : System.Web.Services.WebService
    {


        [WebMethod]
        public string AddEInvoice(Int64 axRecId, Int16 eInvoiceType)
        {
            //axRecId = 5637589351;
            axRecId = 5637543825;
            //axRecId = 5637591075;
            eInvoiceType = 1;
            Helper     helper = new Helper();
            HttpClient client = new HttpClient();
            var a = helper.initEInvoiceSalesData(axRecId, eInvoiceType);

            

            return "OK";
        }

        [WebMethod]
        public void GetIncomingEInvoice()
        {
            new Helper().incomingEInvoice();
        }

        [WebMethod]
        public DataTable CheckGibInvoiceUser(string user)
        {
            DataTable userDt;
            Helper helper = new Helper();
            helper.CheckGibInvoiceUser(user);
            userDt = helper.dt;
            return userDt;
        }
    }
}
