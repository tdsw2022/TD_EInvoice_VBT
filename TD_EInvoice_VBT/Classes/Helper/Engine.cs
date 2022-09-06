using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TD_EInvoice_VBT.Classes.Helper
{
    public class Engine
    {
        public void SendMail(string Body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("\"Tech Data Technology Solutions\" <SystemException.tr@techdata.com>");
            mail.Subject = "[" + ConfigurationManager.AppSettings["AxServer"] + "] e-Invoice System Exception";
            mail.Body = Body;
            mail.To.Add(ConfigurationManager.AppSettings["Mail"]);
            SmtpClient SmtpClient = new SmtpClient("relay.akora.com.tr");
            SmtpClient.Send(mail);
        }

        public string OutgoingInvoiceStatusForUser(string faturaDurumu) 
        {
            switch (faturaDurumu)
            {
                case "InvoiceAndXmlCreated": return "Fatura Oluşturuldu";
                case "Signed": return "Gib'e Gönderime Hazır";
                case "WaitingForSendingToGib": return "Gib'e Gönderim Kuyruğunda";
                case "CouldNotSentToGib": return "Gib'e Gönderilemedi";
                case "SentToGib": return "Gib'e Gönderildi, Yanıt Bekleniyor";
                case "GibFailed": return "Gib'te Hata Aldı";
                case "GibReceived": return "Gib Aldı, Alıcıya İletiliyor";
                case "GibCouldNotSendToParty": return "Gib Alıcıya Ulaştıramadı";
                case "SentToParty": return "Alıcıya İletildi, Yanıt Bekleniyor";
                case "PartyReturnedError": return "Alıcı Faturaya Hata Döndü";
                case "PartyReceivedAndWaitingForApproval": return "Alıcıdan Onay / Red Bekleniyor";
                case "Rejected": return "Reddedildi";
                case "Approved": return "Onaylandı";
                default: return "Fatura durumu bulunamadı!";
            }
        }

        public string profileID(int enumValue)
        {
            switch(enumValue)
            {
                case 1: return "TEMELFATURA"; // Ax aaEInvoiceWorkingType Enum değerine karşılık geliyor
                case 2: return "TICARIFATURA"; // Ax aaEInvoiceWorkingType Enum değerine karşılık geliyor
                case 3: return "YOLCUBERABERFATURA"; // Ax aaEInvoiceWorkingType Enum değerine karşılık geliyor
                case 4: return "IHRACAT"; // Ax aaEInvoiceWorkingType Enum değerine karşılık geliyor
                case 5: return "EARSIVFATURA"; //Ax'da karşılığı yok
                case 6: return "KAMU"; //Ax'da karşılığı yok
                case 7: return "HKS"; //Ax'da karşılığı yok
                default: return "EARSIVFATURA"; //Ax'da karşılığı yok
            }
        }

        public string invoiceTypeCode(int enumValue)
        {
            switch (enumValue)
            {
                case 1: return "SATIS"; 
                case 2: return "IADE"; 
                case 5: return "TEVKIFAT"; 
                case 6: return "ISTISNA"; 
                case 8: return "OZELMATRAH";
                default: return ""; 
            }
        }

        public double convertDouble(object value)
        {
            double _value = Math.Round((double)value,2);
            return _value;
        }
    }
}