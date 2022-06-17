using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TD_EInvoice_VBT.Classes.Models;
using Microsoft.Dynamics.BusinessConnectorNet;
using System.Net.Http.Headers;
using System.Data;

namespace TD_EInvoice_VBT.Classes.Helper
{
    public class Helper
    {

        public Account account;
        Axapta axapta;
        Engine engine;
        public Helper()
        {
            engine = new Engine();
            getToken();
            AxaptaConnection();
        }
        //Token için yazılmıştır...
        private async Task<string> postWithoutAuth(string url, string userJsonData)
        {
            HttpClient client = new HttpClient();

            var convertHttpContent = new StringContent(userJsonData, Encoding.UTF8, "application/json");
            var responseVBT = client.PostAsync(url, convertHttpContent).Result;
            string responseToken = await responseVBT.Content.ReadAsStringAsync();
            return responseToken;
        }
        //Gelen tokeni ekleyerek post ediyoruz...
        private async Task<string> postWithAuth(string url, string userJsonData)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-16");
            client.DefaultRequestHeaders.TryAddWithoutValidation("VbtAuthorization", account.Token);
            StringContent postBody = new StringContent(userJsonData, Encoding.UTF8, "application/json");
            var responseVBT = client.PostAsync(url, postBody).Result;
            string responseData = await responseVBT.Content.ReadAsStringAsync();
            return responseData;
        }

        //Güvenlik için token alıyoruz...
        public void getToken()
        {
            try
            {
                account = new Account { Email = ConfigurationManager.AppSettings["UserId"], Password = ConfigurationManager.AppSettings["Password"] };
                string requestJson = JsonConvert.SerializeObject(account);
                string responseJson = postWithoutAuth(ConfigurationManager.AppSettings["URL_Account"], requestJson).Result;
                account = JsonConvert.DeserializeObject<Account>(responseJson);
                account.Message = "";
            }
            catch (Exception ex)
            {
                new Engine().SendMail($"GetToken - {ex.Message}");
            }
        }

        public void AxaptaConnection()
        {
            try
            {
                axapta = new Axapta();

                axapta.LogonAs(ConfigurationManager.AppSettings["AxUserName"],
                               ConfigurationManager.AppSettings["AxDomain"],
                               new System.Net.NetworkCredential(ConfigurationManager.AppSettings["AxUserName"],
                                                                ConfigurationManager.AppSettings["AxPassword"],
                                                                ConfigurationManager.AppSettings["AxDomain"]),
                               ConfigurationManager.AppSettings["AxCompany"],
                               "TR",
                               ConfigurationManager.AppSettings["AxServer"] + ":" + ConfigurationManager.AppSettings["AxPort"],
                               null);
            }
            catch (Exception ex)
            {
                errorMessage = "Axapta bağlantısı kurulamadı!";
                new Engine().SendMail($"AxaptaConnection - {ex.Message}");
            }
        }

        public  string initEInvoiceSalesData(Int64 axRecId, Int16 eInvoiceType)
        {
            if (account.Message != "")
                return "VBT Token bilgileri getirilemedi!";
            if (axapta == null)
                return "Axapta Bağlantısı Kurulamadı!";
            
            AxaptaContainer axConSalesInvoiceRecords = axapta.CallStaticClassMethod("aaEInvoice", "sendSalesInvoiceRecords", axRecId, eInvoiceType) as AxaptaContainer;
            List<string> noteList = new List<string>();
            noteList.Add("test");

            if (axConSalesInvoiceRecords.Count == 5)
            {
                AxaptaContainer axConCustomer = axConSalesInvoiceRecords.get_Item(1) as AxaptaContainer;
                AxaptaContainer axConProducts = axConSalesInvoiceRecords.get_Item(2) as AxaptaContainer;
                AxaptaContainer axConSalesInvoiceHeader = axConSalesInvoiceRecords.get_Item(3) as AxaptaContainer;
                AxaptaContainer axConSalesInvoiceLines = axConSalesInvoiceRecords.get_Item(4) as AxaptaContainer;
                AxaptaContainer axConSalesInvoiceTaxesLines = axConSalesInvoiceRecords.get_Item(5) as AxaptaContainer;

                #region Customer
                List<Id> partyId = new List<Id>()
                {
                    new Id { SchemeId = axConCustomer.get_Item(6).ToString().Length == 11 ? "TCKNs" : "VKN", Value = (string)axConCustomer.get_Item(6) } // Müşteri VKN/TCKN CustTable.VATNum
                };

                Address postalAddress = new Address {
                    StreetName = (string)axConCustomer.get_Item(7), // CustTable.Street
                    BuildingNumber = string.Empty, // Boş
                    CitySubdivisionName = (string)axConCustomer.get_Item(8), // AddressCounty.aaEInvoiceRecId
                    PostalZone = (string)axConCustomer.get_Item(9), // CustTable.ZipCode
                    Country = new Country { IdentificationCode = (string)axConCustomer.get_Item(16) } // AddressCountryRegion.ISOCode
                };

                Contact contact = new Contact {
                    Telephone = (string)axConCustomer.get_Item(10), // CustTable.Phone
                    Telefax = (string)axConCustomer.get_Item(11), // CustTable.TeleFax
                    ElectronicMail = string.Empty // Boş                                       
                };

                PartyTaxScheme partyTaxScheme = new PartyTaxScheme
                {
                    RegistrationName = (string)axConCustomer.get_Item(13), // Ihracat veya Yolcu beraber fatura durumu ? CustTable.Name : Boş
                    TaxScheme = new TaxScheme { Name = (string)axConCustomer.get_Item(4) }  // CustTable.TaxOfficeName
                };

                Party customerParty = new Party {
                    Id = Convert.ToInt64(axConCustomer.get_Item(15)), //CustTable.RecId
                    PartyIdentifications = partyId,
                    PartyName = (string)axConCustomer.get_Item(2), // CustTable.Name                    
                    PostalAddress = postalAddress,
                    Contact = contact,
                    PartyTaxScheme = partyTaxScheme,
                    WebsiteURI = (string)axConCustomer.get_Item(12) // CustTable.URL
                };
               
                CustomerParty customer = new CustomerParty { Party = customerParty }; //Müşteri objesini burada en son topluyoruz...
                #endregion


                #region InvoiceLine
                for (int lines = 1; lines <= axConSalesInvoiceLines.Count; lines++)
                {
                    AxaptaContainer salesLines = (AxaptaContainer)axConSalesInvoiceLines.get_Item(lines);
                    List<InvoiceLine> listInvoiceLine = new List<InvoiceLine>();                    
                    
                    Quantity InvoicedQuantity = new Quantity
                    {
                        Value = (double)salesLines.get_Item(6), // custInvoiceTrans.Qty veya custInvoiceTrans.QtyForFreeText
                        Unitcode = "NIU" //E-Faturada Adet'e karşılık gelen tanım
                    };

                    InvoiceLine invoiceLine = new InvoiceLine {
                        InvoicedQuantity = InvoicedQuantity,
                        Item = new Item { Name = (string)salesLines.get_Item(5) }, // Ax'ta karşılığı itemName todo kontrol edilecek itemName veya itemId gönderilecek testlerde belli olur.
                        Price = (double)salesLines.get_Item(7), // custInvoiceTrans.SalesPriceForFreeText >0 ? custInvoiceTrans.SalesPriceForFreeText :custInvoiceTrans.SalesPrice
                        
                    };
                    listInvoiceLine.Add(invoiceLine);
                }
                #endregion


                /*List<InvoiceLine> listInvoiceLine = new List<InvoiceLine>();
                List<TaxSubtotal> taxsubTotal = new List<TaxSubtotal>();
                for (int axCon = 1; axCon <= axConSalesInvoiceLines.Count; axCon++)
                {
                    AxaptaContainer lines = (AxaptaContainer)axConSalesInvoiceLines.get_Item(axCon);
                    var a = lines.get_Item(3);
                    if (taxsubTotal.Count < 1)
                    {
                        taxsubTotal.Add(new TaxSubtotal
                        {
                            Percent = (double)lines.get_Item(8),
                            TaxAmount = (double)lines.get_Item(11),
                            TaxableAmount = (double)lines.get_Item(12),
                            CalculationSequenceNumeric = 1,
                            TaxCategory = new TaxCategory { TaxScheme = new TaxScheme { Name = "KDV GERCEK", TaxTypeCode = "0015" } }
                        });
                    }
                    InvoiceLine invoiceLine = new InvoiceLine
                    {
                        Id = Convert.ToString(lines.get_Item(3)),
                        Item = new Item { Name = (string)lines.get_Item(4) },
                        InvoicedQuantity = new Quantity { Value = (double)lines.get_Item(6), Unitcode = "NIU" },
                        TaxTotal = new TaxTotal // sorulacak
                        {
                            TaxAmount = (double)lines.get_Item(11),
                            TaxSubtotal = taxsubTotal
                        },
                        AllowanceCharge = null,
                        Delivery = null,
                        LineExtensionAmount = (double)lines.get_Item(12),
                        Price = (double)lines.get_Item(7)
                    };

                    listInvoiceLine.Add(invoiceLine);
                }

                List<TaxTotal> taxHeaderList = new List<TaxTotal>();  // sorulacak
                TaxTotal taxHeader = new TaxTotal {
                    TaxAmount = (double)axConSalesInvoiceHeader.get_Item(26),
                    TaxSubtotal = taxsubTotal
                };*/
                taxHeaderList.Add(taxHeader);
                MonetaryTotal monetaryTotal = new MonetaryTotal { LineExtensionAmount = 1, TaxExclusiveAmount = 1, TaxInclusiveAmount = 1.18, AllowanceTotalAmount = 0, ChargeTotalAmount = 0, PayableAmount = 1.18 };

                OutgoingInvoice invoiceHeader = new OutgoingInvoice
                {
                    //LocationCode = (string)axConSalesInvoiceHeader.get_Item(34),
                    //XsltFileName = "",
                    //FirmBranchCode = "",
                    //TryCountDescription = "",
                    //TryCount = 0,
                    EArchiveMailTo = (string)axConSalesInvoiceHeader.get_Item(33),
                    //PutInvoiceNumberIntoQrCode = false,
                    //MailTo = "",
                    ReceiverIdentifier = "", // sorulacak
                    //ChannelCode = "",
                    //PartyGroupId = 0,
                    //PartyGroupName = "",
                    //IsOocumentNumberIncluded = false,
                    //Barcode = null,
                    //Id = 0,
                    InvoiceExternalId = (string)axConSalesInvoiceHeader.get_Item(1),
                    //InvoiceNumber = "",
                    ProfileId = engine.profileID(Convert.ToInt16(axConCustomer.get_Item(3))),
                    //UUId = "",
                    InvoiceTypeCode = "SATIS", //// sorulacak
                    DocumentCurrencyCode = (string)axConSalesInvoiceHeader.get_Item(36),
                    CopyIndicator = false,
                    IssueDate = DateTime.Now,//Convert.ToDateTime(axConSalesInvoiceHeader.get_Item(8)).AddHours(1),
                    LineCountNumeric = listInvoiceLine.Count,
                    Note = noteList,
                    //AccountingSupplierParty = null,
                    AccountingCustomerParty = customer,
                    AllowanceCharge = null,
                    TaxTotal = taxHeaderList,
                    WithholdingTaxTotal = null,//new List<TaxTotal>(),//taxHeaderList,
                    LegalMonetaryTotal = monetaryTotal,
                    InvoiceLine = listInvoiceLine,
                    //AccountingCost = "",
                    //TaxCurrencyCode = "",
                    //PricingCurrencyCode = "",
                    //PaymentCurrencyCode = "",
                    //PaymentAlternativeCurrencyCode = "",
                    //InvoicePeriod = null,
                    //OrderReference = null,
                    BillingReference = null,
                    DespatchDocumentReference = null,
                    //ReceiptDocumentReference = null,
                    //OriginatorDocumentReference = null,
                    //ContractDocumentReference = null,
                    AdditionalDocumentReference = null,
                    BuyerCustomerParty = null,
                    //SellerSupplierParty = null,
                    //TaxRepresentativeParty = null,
                    //Delivery = null,
                    //PaymentMeans = null,
                    //PaymentTerms = null,
                    //TaxExchangeRate = null,
                    PricingExchangeRate = new ExchangeRate { SourceCurrencyCode = (string)axConSalesInvoiceHeader.get_Item(36), TargetCurrencyCode = "TRY", CalculationRate = Math.Round((double)axConSalesInvoiceHeader.get_Item(35), 4), Date = Convert.ToDateTime(axConSalesInvoiceHeader.get_Item(4)) },
                    //PaymentExchangeRate = null,
                    //PaymentAlternativeExchangeRate = null,
                    InternetPayment = null,
                    //DatePosted = DateTime.Now
                };

                axapta.Logoff();

                var requestJson = JsonConvert.SerializeObject(invoiceHeader);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_AddEInvoice"], requestJson).Result;
                OutgoingInvoiceResponse response = JsonConvert.DeserializeObject<OutgoingInvoiceResponse>(responseJson);
                if (response.Data.HasError)
                    return response.Data.Errors.Min().ErrorMessage.ToString();

                return response.Data.InvoiceNumber + "numaralı fatura E-Fatura sistemine eklenmiştir.";
            }
            else
            {
                return "Fatura kayıt numaralı bilgiler eksik veya bulunamadı. (SendSalesInvoiceRecords)" + "- axConSalesInvoiceRecords = " + axConSalesInvoiceRecords.Count;
            }

            
        }

        //Gelen E-Fatura
        public string incomingEInvoice()
        {
            try
            {
                DateTime startDate = DateTime.Now; startDate = startDate.AddDays(-30);
                DateTime endDate = startDate.AddDays(60);

                IncomingInvoice incomingEInvoice = new IncomingInvoice
                {
                    Query = new IncomingInvoiceQuery
                    {
                        IssueDate = new DateRange { StartDate = startDate, EndDate = endDate },
                        IsErpProcessed = false
                    },
                    Take = 100
                };

                string requestJson = JsonConvert.SerializeObject(incomingEInvoice);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_GetIncomingEInvoice"], requestJson).Result;
                IncomingInvoiceResponse response = JsonConvert.DeserializeObject<IncomingInvoiceResponse>(responseJson);
                if (String.IsNullOrEmpty(response.RefreshToken))
                    return response.Message;
                //List<IncomingInvoiceDataResponse> dataResponse = 
                List<IncomingInvoiceResponseDetail> res = response.Data.Results;
                /*if (!String.IsNullOrEmpty(dataResponse.ErrorCode))
                    return dataResponse.Message;
                if(dataResponse.Data.Total > 0)
                {
                    foreach(IncomingInvoiceResponse resp in response)
                    {
                        
                    }
                }*/
                //return $"Toplamda {dataResponse.Data.Total} kayıt aktarıldı...";*/
            }
            catch (Exception Ex)
            {

            }
            return "Tüm kayıtlar aktarılmış...";
        }

        //E-Fatura iptal
        public string deleteOutgoingEInvoice(string _ettn)
        {
            var deleteOutgoingEInvoiceRequest = new { Ettn = _ettn };
            var deleteOutgoingEInvoiceResponse = new { refreshToken = string.Empty, Data = false, Message = string.Empty };
            try
            {
                string requestJson = JsonConvert.SerializeObject(deleteOutgoingEInvoiceRequest);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_DeleteEInvoice"], requestJson).Result;
                deleteOutgoingEInvoiceResponse = JsonConvert.DeserializeAnonymousType(responseJson, deleteOutgoingEInvoiceResponse);
            }
            catch (Exception ex)
            {                
                new Engine().SendMail($"deleteOutgoingEInvoice - {ex.Message}");
                return "E-Fatura iptal işlemi gerçekleştirilemedi!";
            }

            if (deleteOutgoingEInvoiceResponse.Data) // Başarılı bir şekilde iptal edildi mi kontrol ediliyor.
                return "E-Fatura iptal edilmiştir";
            return deleteOutgoingEInvoiceResponse.Message; //Fatura iptal edilemediyse dönen mesajı axaptaya dönüyoruz.
        }

        //E-Fatura PDF
        public string getEInvoicePDFUrl(string _ettn)
        {
            List<String> listEttn = new List<string> { _ettn };
            var pdfRequest = new { Ettns = listEttn };
            var PdfList = new[] { new { DownloadUrl = String.Empty } }.ToList();
            var data = new { PdfList = PdfList };
            var pdfResponse = new { Data = data };

            try
            {
                string requestJson = JsonConvert.SerializeObject(pdfRequest);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_GetEInvoicePDF"], requestJson).Result;
                pdfResponse = JsonConvert.DeserializeAnonymousType(responseJson, pdfResponse);
            }
            catch (Exception ex)
            {
                errorMessage = "PDF indirilemedi!";
                new Engine().SendMail($"getEInvoicePDFUrl - {ex.Message}");
                return errorMessage;
            }
            return pdfResponse.Data.PdfList[0].DownloadUrl;
        }

        //Giden E-Fatura Statü bilgisini döner
        public string getEInvoiceStatus(string _ettn)
        {
            List<String> listEttn = new List<string> { _ettn };
            var pdfRequest = new { EttnList = listEttn };
            var InvoiceStatusList = new[] { new { OutgoingInvoiceStatus = String.Empty } }.ToList();
            var data = new { InvoiceStatusList = InvoiceStatusList };
            var pdfResponse = new { Data = data };

            try
            {
                string requestJson = JsonConvert.SerializeObject(pdfRequest);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_GetEInvoiceStatus"], requestJson).Result;
                pdfResponse = JsonConvert.DeserializeAnonymousType(responseJson, pdfResponse);
            }
            catch (Exception ex)
            {
                errorMessage = "E-Fatura Statüsünde hata!";
                new Engine().SendMail($"getEInvoiceStatus - {ex.Message}");
                return errorMessage;
            }

            return new Engine().OutgoingInvoiceStatusForUser(pdfResponse.Data.InvoiceStatusList[0].OutgoingInvoiceStatus);
        }

        //Mükellef sorgulama
        public DataTable CheckGibInvoiceUser(string user)
        {
            Taxpayer taxpayer = new Taxpayer { Identifier = user };
            DataTable userResponseDt = new DataTable();
            try
            {
                string requestJson = JsonConvert.SerializeObject(taxpayer);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_GetGibInvoiceUser"], requestJson).Result;
                UserResponse response = JsonConvert.DeserializeObject<UserResponse>(responseJson);
                /*userResponseDt.Columns.Add()
                if (response.Data == null)
                {                    
                }
                else
                {
                }*/
            }
            catch (Exception ex)
            {
                new Engine().SendMail($"CheckGibInvoiceUser - {ex.Message}");
            }
            return userResponseDt;
        }


    }
}   
