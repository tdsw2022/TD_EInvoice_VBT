using System;
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
            Helper helper = new Helper();
            if (String.IsNullOrEmpty(helper.account.Token))
                return "Token alınamadı!";
            //axRecId = 5637589351;
            axRecId = 5637543825;
            //axRecId = 5637591075;
            eInvoiceType = 1;
            
            string respMessage = helper.initEInvoiceSalesData(axRecId, eInvoiceType);

            return respMessage;
        }

        [WebMethod]
        public string DeleteEInvoice(string Ettn)
        {             
            Helper helper = new Helper();
            if (String.IsNullOrEmpty(helper.account.Token))
                return "Token alınamadı!";

            string resp = helper.deleteOutgoingEInvoice(Ettn);

            return resp;
        }

        [WebMethod]
        public dynamic GetIncomingEInvoice()
        {
            Helper helper = new Helper();
            if (String.IsNullOrEmpty(helper.account.Token))
                return "Token alınamadı!";
            helper.incomingEInvoice();
            return "";
        }

        [WebMethod]
        public string CheckGibInvoiceUser(string user)
        {
            Helper helper = new Helper();
            if (String.IsNullOrEmpty(helper.account.Token))
                return "Token alınamadı!";
            helper.CheckGibInvoiceUser(user);
            //userDt = helper.dt;
            return "";
        }

        [WebMethod]
        public string getEInvoicePDF(string ettn)
        {
            Helper helper = new Helper();
            if (String.IsNullOrEmpty(helper.account.Token))
                return "Token alınamadı!";
            string URL = helper.getEInvoicePDFUrl(ettn);
            return URL;
        }

        [WebMethod]
        public string getEInvoiceStatus(string ettn)
        {
            Helper helper = new Helper();
            if (String.IsNullOrEmpty(helper.account.Token))
                return "Token alınamadı!";
            string response = helper.getEInvoiceStatus(ettn);
            return response;
        }
    }
}
