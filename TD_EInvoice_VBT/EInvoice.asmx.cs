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
        public string AddEInvoice(string parametre)
        {
            Int16 eInvoiceType = 0;
            Int64 axRecId = 0;
            if (parametre.Length == 12)
            {
                axRecId = Convert.ToInt64(parametre.Remove(parametre.Length - 2, 2));
                eInvoiceType = Convert.ToInt16(parametre.Remove(0, parametre.Length - 1));
            }
            else if (parametre.Length == 13)
            {
                axRecId = Convert.ToInt64(parametre.Remove(parametre.Length - 3, 3));
                eInvoiceType = Convert.ToInt16(parametre.Remove(0, parametre.Length - 2));
            }
            Helper helper = new Helper();

            if (String.IsNullOrEmpty(helper.account.Token))
                return "VBT Token bilgileri getirilemedi!";
            if (helper.axapta == null)
                return "Axapta Bağlantısı Kurulamadı!";
            
            string respMessage = helper.sendEInvoice(axRecId, eInvoiceType);

            return respMessage;
        }

        [WebMethod]
        public string DeleteEInvoice(string Ettn)
        {             
            Helper helper = new Helper();

            if (String.IsNullOrEmpty(helper.account.Token))
                return "VBT Token bilgileri getirilemedi!";
            if (helper.axapta == null)
                return "Axapta Bağlantısı Kurulamadı!";

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
        public string CheckGibInvoiceUser(string vkn)
        {
            Helper helper = new Helper();

            if (String.IsNullOrEmpty(helper.account.Token))
                return "VBT Token bilgileri getirilemedi!";
            if (helper.axapta == null)
                return "Axapta Bağlantısı Kurulamadı!";

            string response = helper.CheckGibInvoiceUser(vkn);

            return response;
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
                return "VBT Token bilgileri getirilemedi!";

            string response = helper.getEInvoiceStatus(ettn);
            return response;
        }
    }
}
