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
        public DataTable dt;
        
        public async Task<Account> getToken()
        {
            Account account = new Account { Email = ConfigurationManager.AppSettings["UserId"], Password = ConfigurationManager.AppSettings["Password"] };
            try
            {
                HttpClient client = new HttpClient();

                string userJsonData = JsonConvert.SerializeObject(account);
                var convertHttpContent = new StringContent(userJsonData, Encoding.UTF8, "application/json");

                var responseVBT = client.PostAsync(ConfigurationManager.AppSettings["URL_Account"], convertHttpContent).Result;

                string responseToken = await responseVBT.Content.ReadAsStringAsync();

                account = JsonConvert.DeserializeObject<Account>(responseToken);
                account.Message = "";
            }
            catch (Exception Ex)
            {
                account.Message = Ex.Message;
                return account;
            }
            return account;
        }

        public Axapta AxaptaConnection()
        {
            Axapta result = default;
            try
            {
                result = new Axapta();

                result.LogonAs(ConfigurationManager.AppSettings["AxUserName"],
                               ConfigurationManager.AppSettings["AxDomain"],
                               new System.Net.NetworkCredential(ConfigurationManager.AppSettings["AxUserName"],
                                                                ConfigurationManager.AppSettings["AxPassword"],
                                                                ConfigurationManager.AppSettings["AxDomain"]),
                               ConfigurationManager.AppSettings["AxCompany"],
                               "TR",
                               ConfigurationManager.AppSettings["AxServer"] + ":" + ConfigurationManager.AppSettings["AxPort"],
                               null);
            }
            catch
            {
                return null;
            }
            return result;
        }

        public async Task<string> initEInvoiceSalesData(Int64 axRecId, Int16 eInvoiceType)
        {
            Axapta axapta = AxaptaConnection();
            Account account = await getToken();
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

                List<Id> partyId = new List<Id>();
                partyId.Add(new Id { SchemeId = "VKN", Value = (string)axConCustomer.get_Item(6) });
                string asd = "TR";//(string)axConCustomer.get_Item(8);
                Party customerParty = new Party
                {
                    Id = 1,
                    WebsiteURI = (string)axConCustomer.get_Item(12),
                    PartyIdentifications = partyId,
                    PartyName = (string)axConCustomer.get_Item(2),
                    PostalAddress = new Address
                    {
                        Id = null, Postbox = null, Room = null, BlockName = null, BuildingName = null, BuildingNumber = null, CitySubdivisionName = null, CityName = null, PostalZone = null, Region = null, District = null, Country = new Country { IdentificationCode = asd },//(string)axConCustomer.get_Item(8) }, İSO KODU PROBLEMİ
                        StreetName = (string)axConCustomer.get_Item(7)
                    },
                    Contact = new Contact
                    {
                        Id = null, Name = null, Telephone = (string)axConCustomer.get_Item(10), Telefax = (string)axConCustomer.get_Item(11), ElectronicMail = null, Note = null, OtherCommunication = null
                    },
                    PartyTaxScheme = null,
                    Person = null,
                    EndpointId = "",
                    IndustryClassificationCode = "",
                    PhysicalLocation = null,
                    PartyLegalEntity = null,
                    AgentParty = null,
                    OpenningBalanceDate = default,
                    OpenningBalance = 0,
                    CategoryId = 0,
                    IdentityOrTaxNumber = "11111111111",
                    LegalOrPerson = "G",
                    EArchiveMailTo = ""
                };
                CustomerParty customer = new CustomerParty { Party = customerParty, DeliveryContact = null };
                List<InvoiceLine> listInvoiceLine = new List<InvoiceLine>();
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
                };
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
                    ProfileId = "TEMELFATURA",//"TICARIFATURA",//"TEMELFATURA", //// sorulacak
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
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-16");
                client.DefaultRequestHeaders.TryAddWithoutValidation("VbtAuthorization", account.Token);

                var requestBody = JsonConvert.SerializeObject(invoiceHeader);
                StringContent postBody = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var responseVBT = client.PostAsync(ConfigurationManager.AppSettings["URL_AddEInvoice"], postBody).Result;
                string responseData = await responseVBT.Content.ReadAsStringAsync();

                //OutgoingInvoice adviceResponse = JsonConvert.DeserializeObject<OutgoingInvoice>(responseData);
            }
            else
            {
                return "Fatura kayıt numaralı bilgiler eksik veya bulunamadı. (SendSalesInvoiceRecords)" + "- axConSalesInvoiceRecords = " + axConSalesInvoiceRecords.Count;
            }

            axapta.Logoff();
            return "";
        }

        public async Task<string> incomingEInvoice()
        {
            try
            {
                DateTime startDate = DateTime.Now; startDate = startDate.AddDays(-30);
                DateTime endDate = startDate.AddDays(60);
                HttpClient client = new HttpClient();
                Account account = await getToken();

                IncomingInvoice incomingEInvoice = new IncomingInvoice
                {
                    Query = new IncomingInvoiceQuery
                    {
                        IssueDate = new DateRange { StartDate = startDate, EndDate = endDate },
                        IsErpProcessed = false
                    },
                    Take = 100
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-16");
                client.DefaultRequestHeaders.TryAddWithoutValidation("VbtAuthorization", account.Token);

                var requestBody = JsonConvert.SerializeObject(incomingEInvoice);
                StringContent postBody = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var responseVBT = client.PostAsync(ConfigurationManager.AppSettings["URL_GetIncomingEInvoice"], postBody).Result;
                string responseData = await responseVBT.Content.ReadAsStringAsync();

                IncomingInvoiceResponse response = JsonConvert.DeserializeObject<IncomingInvoiceResponse>(responseData);
                //List<IncomingInvoiceDataResponse> dataResponse = 
                /*List<IncomingInvoiceDataResponse> response = dataResponse.Data.Results;
                if (!String.IsNullOrEmpty(dataResponse.ErrorCode))
                    return dataResponse.Message;
                if(dataResponse.Data.Total > 0)
                {
                    foreach(IncomingInvoiceResponse resp in response)
                    {
                        
                    }
                }
                return $"Toplamda {dataResponse.Data.Total} kayıt aktarıldı...";*/
            }
            catch(Exception Ex)
            {
                
            }
            return "Tüm kayıtlar aktarılmış...";
        }

        public async Task CheckGibInvoiceUser(string user)
        {
            Account account = await getToken();
            Taxpayer taxpayer = new Taxpayer { Identifier = user };
            AxaptaContainer ax = default;

            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applications/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-16");
                client.DefaultRequestHeaders.TryAddWithoutValidation("VbtAuthorization", account.Token);

                var requestBody = JsonConvert.SerializeObject(taxpayer);
                StringContent postBody = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var responseVBT = client.PostAsync(ConfigurationManager.AppSettings["URL_GetGibInvoiceUser"], postBody).Result;
                string responseData = await responseVBT.Content.ReadAsStringAsync();

                UserResponse response = JsonConvert.DeserializeObject<UserResponse>(responseData);
                
                if (response.Data == null)
                {                    
                }
                else
                {
                }
            }
            catch (Exception Ex)
            {
            }
        }
       
        
    }

    
}