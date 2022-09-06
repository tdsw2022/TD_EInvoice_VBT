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
using System.Net;

namespace TD_EInvoice_VBT.Classes.Helper
{
    public class Helper
    {

        public Account account;
        public Axapta axapta;
        Engine engine;
        AxaptaContainer axConCustomer;
        AxaptaContainer axConProducts;
        AxaptaContainer axConSalesInvoiceHeader;
        AxaptaContainer axConSalesInvoiceLines;
        AxaptaContainer axConSalesInvoiceTaxesLines;
        public Helper()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                engine.SendMail($"GetToken - {ex.Message}");
            }
        }

        //Axapta bağlantısı yapıyoruz...
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
                axapta = null;
                engine.SendMail($"AxaptaConnection - {ex.Message}");
            }
        }

        //Vergi satırlarını model ekliyoruz. Bunun sebebi fatura satırlarında gezerken doğru satırı direk bulabilecek...
        public List<AxaptaTaxLines> initTaxLines()
        {
            List<AxaptaTaxLines> listTaxLines = new List<AxaptaTaxLines>();

            for (int taxLines = 2; taxLines <= axConSalesInvoiceTaxesLines.Count; taxLines++)
            {
                AxaptaContainer taxLine = (AxaptaContainer)axConSalesInvoiceTaxesLines.get_Item(taxLines);

                listTaxLines.Add
                (
                    new AxaptaTaxLines 
                    { 
                        custInvoiceTransRecId = Convert.ToInt64(taxLine.get_Item(1)), //CustInvoiceTrans.RecId
                        taxRecId = Convert.ToString(taxLine.get_Item(2)), //TaxTrans.RecId
                        eInvoiceRecId = Convert.ToString(taxLine.get_Item(3)), //taxOnItem.aaEInvoiceRecId
                        taxValue = Convert.ToString(taxLine.get_Item(4)), // Ax tarafında hesaplama
                        sourceTaxAmountCur = engine.convertDouble(taxLine.get_Item(5)) // Ax tarafında hesaplama
                    }

                );
            }

            return listTaxLines;
        }

        //E-Fatura gönderim
        public  string sendEInvoice(Int64 axRecId, Int16 eInvoiceType)
        {

            try
            {
                AxaptaContainer axConSalesInvoiceRecords = axapta.CallStaticClassMethod("aaEInvoice", "sendSalesInvoiceRecords", axRecId, eInvoiceType) as AxaptaContainer;
                List<string> noteList = new List<string>();
                noteList.Add("test");

                if (axConSalesInvoiceRecords.Count == 5)
                {
                    axConCustomer = axConSalesInvoiceRecords.get_Item(1) as AxaptaContainer;
                    axConProducts = axConSalesInvoiceRecords.get_Item(2) as AxaptaContainer;
                    axConSalesInvoiceHeader = axConSalesInvoiceRecords.get_Item(3) as AxaptaContainer;
                    axConSalesInvoiceLines = axConSalesInvoiceRecords.get_Item(4) as AxaptaContainer;
                    axConSalesInvoiceTaxesLines = axConSalesInvoiceRecords.get_Item(5) as AxaptaContainer;
                    double taxPercent = default;

                    #region Customer
                    List<Id> partyId = new List<Id>()
                    {
                        new Id { SchemeId = axConCustomer.get_Item(6).ToString().Length == 11 ? "TCKN" : "VKN", Value = (string)axConCustomer.get_Item(6) } // Müşteri VKN/TCKN CustTable.VATNum
                    };

                    Address postalAddress = new Address
                    {
                        StreetName = (string)axConCustomer.get_Item(7), // CustTable.Street
                        BuildingNumber = string.Empty, // Boş
                        CitySubdivisionName = (string)axConCustomer.get_Item(8), // AddressCounty.aaEInvoiceRecId
                        PostalZone = (string)axConCustomer.get_Item(9), // CustTable.ZipCode
                        Country = new Country { IdentificationCode = (string)axConCustomer.get_Item(16) } // AddressCountryRegion.ISOCode
                    };

                    Contact contact = new Contact
                    {
                        Telephone = (string)axConCustomer.get_Item(10), // CustTable.Phone
                        Telefax = (string)axConCustomer.get_Item(11), // CustTable.TeleFax
                        ElectronicMail = string.Empty // Boş                                       
                    };

                    PartyTaxScheme partyTaxScheme = new PartyTaxScheme
                    {
                        RegistrationName = (string)axConCustomer.get_Item(13), // Ihracat veya Yolcu beraber fatura durumu ? CustTable.Name : Boş
                        TaxScheme = new TaxScheme { Name = (string)axConCustomer.get_Item(4)}  // CustTable.TaxOfficeName
                    };

                    Party customerParty = new Party
                    {
                        Id = Convert.ToInt64(axConCustomer.get_Item(15)), //CustTable.RecId
                        PartyIdentifications = partyId,
                        PartyName = (string)axConCustomer.get_Item(2), // CustTable.Name                    
                        PostalAddress = postalAddress,
                        Contact = contact,
                        PartyTaxScheme = partyTaxScheme,
                        Person = new Person { FirstName = (string)axConCustomer.get_Item(2), FamilyName = (string)axConCustomer.get_Item(2) },
                        WebsiteURI = (string)axConCustomer.get_Item(12) // CustTable.URL
                    };

                    CustomerParty customer = new CustomerParty { Party = customerParty }; //Müşteri objesini burada en son topluyoruz...
                    #endregion

                    #region InvoiceLine

                    List<InvoiceLine> listInvoiceLine = new List<InvoiceLine>();
                    List<AxaptaTaxLines> taxLines = initTaxLines(); // Bir satıra ait ekstra vergi satırı tanımı varsa ekleme yapıyoruz...
                    List<TaxSubtotal> listInvoiceTaxLinesWithholding = new List<TaxSubtotal>();
                    Double  totalTaxAmountWithholding = default;

                    for (int lines = 1; lines <= axConSalesInvoiceLines.Count; lines++)
                    {
                        AxaptaContainer salesLines = (AxaptaContainer)axConSalesInvoiceLines.get_Item(lines);

                        List<TaxSubtotal> listInvoiceTaxLines = new List<TaxSubtotal>();

                        Double totalTaxAmount = default;

                        #region Tax
                        List<AxaptaTaxLines> taxs = taxLines.Where(x => x.custInvoiceTransRecId == (Int64)salesLines.get_Item(2)).ToList();
                        
                        //Ekstra vergi                                            
                        foreach (AxaptaTaxLines salesLineTaxes in taxs)
                        {
                            listInvoiceTaxLinesWithholding.Add(new TaxSubtotal
                            {
                                Percent = Convert.ToInt32(salesLineTaxes.taxValue),
                                TaxAmount = salesLineTaxes.sourceTaxAmountCur,
                                TaxCategory = new TaxCategory { TaxScheme = new TaxScheme { Name = Convert.ToString(salesLineTaxes.eInvoiceRecId), TaxTypeCode = salesLineTaxes.eInvoiceRecId } }
                            });
                            totalTaxAmountWithholding += salesLineTaxes.sourceTaxAmountCur;
                        }

                        //Fatura satırının vergisi
                        listInvoiceTaxLines.Add(new TaxSubtotal
                        {
                            Percent = Convert.ToInt32(salesLines.get_Item(11)),//engine.convertDouble(salesLines.get_Item(11)),
                            TaxAmount = engine.convertDouble(salesLines.get_Item(12)),
                            TaxCategory = new TaxCategory { TaxScheme = new TaxScheme { Name = (string)salesLines.get_Item(13), TaxTypeCode = (string)salesLines.get_Item(13) } }
                        });
                        totalTaxAmount += Convert.ToDouble(salesLines.get_Item(12));
                        taxPercent = engine.convertDouble(salesLines.get_Item(11));
                        #endregion

                        #region Delivery
                        Address deliveryAddress = new Address
                        {
                            CitySubdivisionName = (string)salesLines.get_Item(22),
                            CityName = (string)salesLines.get_Item(21),
                            Country = new Country { Name = (string)salesLines.get_Item(20) }
                        };

                        List<DeliveryTerms> deliveryTerms = new List<DeliveryTerms>
                        {
                            new DeliveryTerms
                            {
                                Id = new Id { Value = (string)salesLines.get_Item(14) }
                            }
                        };

                        double actualPackageValue = 0;
                        if ((string)salesLines.get_Item(15) == "" || (string)salesLines.get_Item(15) == "0")
                            actualPackageValue = 0;
                        else
                            actualPackageValue = Convert.ToDouble(salesLines.get_Item(15));

                        Shipment shipment = new Shipment
                        {
                            GoodsItem = new List<GoodsItem>
                        {
                            new GoodsItem
                            {
                                RequiredCustomsId = (string)salesLines.get_Item(23)
                            }
                        },
                            ShipmentStage = new List<ShipmentStage>
                        {
                            new ShipmentStage
                            {
                                TransportModeCode = (string)salesLines.get_Item(19)
                            }
                        },
                            TransportHandlingUnit = new List<TransportHandlingUnit>
                        {
                            new TransportHandlingUnit
                            {
                                ActualPackage = new List<Package>
                                {
                                    new Package
                                    {
                                        Id = (string)salesLines.get_Item(18),
                                        Quantity = new Quantity { Value = actualPackageValue },
                                        PackaingTypeCode = (string)salesLines.get_Item(16)
                                    }
                                }
                            }
                        }
                        };

                        List<Delivery> delivery = new List<Delivery>
                    {
                        new Delivery
                        {
                            DeliveryAddress = deliveryAddress,
                            DeliveryTerms = deliveryTerms,
                            Shipment = shipment
                        }
                    };
                        #endregion

                        Quantity InvoicedQuantity = new Quantity
                        {
                            Value = Convert.ToDouble(salesLines.get_Item(6)), // custInvoiceTrans.Qty veya custInvoiceTrans.QtyForFreeText
                            Unitcode = "NIU" //E-Faturada Adet'e karşılık gelen tanım
                        };

                        List<AllowanceCharge> allowanceCharge = new List<AllowanceCharge>
                    {
                        new AllowanceCharge
                        {
                            MultiplierFactorNumeric = Convert.ToDouble(salesLines.get_Item(8)),
                            Amount = Convert.ToDouble(salesLines.get_Item(9))
                        }
                    };

                        InvoiceLine invoiceLine = new InvoiceLine
                        {
                            InvoicedQuantity = InvoicedQuantity,
                            Item = new Item { Name = (string)salesLines.get_Item(5) }, // Ax'ta karşılığı itemName todo kontrol edilecek itemName veya itemId gönderilecek.
                            Price = (double)salesLines.get_Item(7), // custInvoiceTrans.SalesPriceForFreeText >0 ? custInvoiceTrans.SalesPriceForFreeText :custInvoiceTrans.SalesPrice
                            TaxTotal = new TaxTotal { TaxAmount = totalTaxAmount, TaxSubtotal = listInvoiceTaxLines },
                            Id = Convert.ToString(salesLines.get_Item(3)),
                            Delivery = delivery,
                            AllowanceCharge = allowanceCharge,
                            LineExtensionAmount = Convert.ToDouble(salesLines.get_Item(10)),
                            WithholdingTaxTotal = listInvoiceTaxLinesWithholding.Count == 0 ? null : new List<TaxTotal> { new TaxTotal { TaxSubtotal = listInvoiceTaxLinesWithholding, TaxAmount = totalTaxAmountWithholding } }//new List<TaxTotal>(),//taxHeaderList,
                        };

                        listInvoiceLine.Add(invoiceLine);
                    }
                    #endregion

                    List<TaxTotal> taxHeaderList = new List<TaxTotal>
                {
                    new TaxTotal
                    {
                        TaxAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(26)), // Toplam vergi
                        TaxSubtotal = new List<TaxSubtotal>
                        {
                            new TaxSubtotal
                            {
                                Percent = Convert.ToInt32(taxPercent),
                                TaxAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(26)), // Toplam vergi
                                TaxableAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(27)), //Ödenecek Tutar                                
                                TaxCategory = new TaxCategory { TaxScheme = new TaxScheme { Name = "KDV GERCEK", TaxTypeCode = "0015" } }
                            }
                        }
                    }
                };

                    #region Sub Total
                    MonetaryTotal monetaryTotal = new MonetaryTotal
                    {
                        LineExtensionAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(23)), // Mal Hizmet Toplam Tutarı	                   
                        AllowanceTotalAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(25)), // Toplam İskonto	                     
                        TaxInclusiveAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(27)), // Vergiler Dahil Toplam Tutar	
                        PayableAmount = engine.convertDouble(axConSalesInvoiceHeader.get_Item(27)) //Ödenecek Tutar	
                    };
                    #endregion

                    OutgoingInvoice invoiceHeader = new OutgoingInvoice
                    {
                        //LocationCode = (string)axConSalesInvoiceHeader.get_Item(34),
                        //XsltFileName = "",
                        //FirmBranchCode = "",
                        //TryCountDescription = "",
                        //TryCount = 0,
                        EArchiveMailTo = engine.profileID(Convert.ToInt16(axConSalesInvoiceHeader.get_Item(3))) == "EARSIVFATURA" ? "mert.guzey@techdata.com" : "", //(string)axConSalesInvoiceHeader.get_Item(33), canlıda açılacak
                                                                                                                                                                    //PutInvoiceNumberIntoQrCode = false,
                                                                                                                                                                    //MailTo = "",
                        ReceiverIdentifier = "", // canlıd açılacak get item yapılıp
                                                 //Barcode = null,
                                                 //Id = 0,
                        InvoiceExternalId = (string)axConSalesInvoiceHeader.get_Item(1),
                        //InvoiceNumber = "",
                        ProfileId = engine.profileID(Convert.ToInt16(axConSalesInvoiceHeader.get_Item(3))),
                        InvoiceTypeCode = engine.invoiceTypeCode(Convert.ToInt16(axConSalesInvoiceHeader.get_Item(5))),
                        DocumentCurrencyCode = (string)axConSalesInvoiceHeader.get_Item(36),
                        CopyIndicator = false,
                        IssueDate = DateTime.Now,//Convert.ToDateTime(axConSalesInvoiceHeader.get_Item(8)).AddHours(1), canlıda açılacak
                        LineCountNumeric = listInvoiceLine.Count,
                        Note = noteList,
                        AccountingCustomerParty = customer,
                        AllowanceCharge = null,
                        TaxTotal = taxHeaderList,
                        WithholdingTaxTotal = null,//new List<TaxTotal> { new TaxTotal { TaxSubtotal = listInvoiceTaxLinesWithholding, TaxAmount = totalTaxAmountWithholding} },//new List<TaxTotal>(),//taxHeaderList,
                        LegalMonetaryTotal = monetaryTotal,
                        InvoiceLine = listInvoiceLine,
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
                        //DatePosted = DateTime.Now*/
                    };



                    var requestJson = JsonConvert.SerializeObject(invoiceHeader);
                    string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_AddEInvoice"], requestJson).Result;
                    OutgoingInvoiceResponse response = JsonConvert.DeserializeObject<OutgoingInvoiceResponse>(responseJson);



                    if (response.Data.HasError)
                    {
                        axapta.Logoff();
                        return response.Data.Errors[0].ErrorMessage; ;
                    }

                    this.setAxaptaInvoiceValues(response); //Axaptadaki bilgileri günceller
                    axapta.Logoff();

                    return response.Data.InvoiceNumber + " numaralı fatura sisteme eklenmiştir.-1";
                }
                else
                {
                    return "Fatura kayıt numaralı bilgiler eksik veya bulunamadı. (SendSalesInvoiceRecords)" + "- axConSalesInvoiceRecords = " + axConSalesInvoiceRecords.Count;
                }
            }
            catch(Exception ex)
            {
                new Engine().SendMail($"sendEInvoice - {ex.Message}");
                return ex.Message;
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
                new Engine().SendMail($"getEInvoicePDFUrl - {ex.Message}");
                return "PDF indirilemedi!";
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
                new Engine().SendMail($"getEInvoiceStatus - {ex.Message}");
                return "E-Fatura Statüsünde hata!";
            }

            return engine.OutgoingInvoiceStatusForUser(pdfResponse.Data.InvoiceStatusList[0].OutgoingInvoiceStatus);
        }

        //Mükellef sorgulama
        public string CheckGibInvoiceUser(string vkn)
        {
            Taxpayer taxpayer = new Taxpayer { Identifier = vkn };
            string etiket = default ;
            try
            {
                string requestJson = JsonConvert.SerializeObject(taxpayer);
                string responseJson = postWithAuth(ConfigurationManager.AppSettings["URL_GetGibInvoiceUser"], requestJson).Result;
                UserResponse response = JsonConvert.DeserializeObject<UserResponse>(responseJson);
                if (response.Data != null)
                {
                    foreach (DocumentResponse documentResponse in response.Data.Documents)
                    {
                        if (documentResponse.Type == "Invoice")
                        {
                            etiket = documentResponse.Aliases[0].Name;
                            axapta.CallStaticClassMethod("aaEInvoice", "updateCustVendInformation", vkn, true, Convert.ToDateTime(documentResponse.Aliases[0].CreationTime).Date, Convert.ToDateTime(documentResponse.Aliases[0].DeletionTime).Date, etiket);
                            axapta.Logoff();
                        }
                    }
                }
                else
                    return "Mükellef e-fatura sistemine dahil değildir.";

                return "Mükellef bilgileri güncellendi.";
            }
            catch (Exception ex)
            {
                engine.SendMail($"CheckGibInvoiceUser - {ex.Message}");
                return "Mükellef sorgulama esnasında hata oluştu!";
            }            
        }

        public void setAxaptaInvoiceValues(OutgoingInvoiceResponse response)
        {
            AxaptaContainer axconStatusList = axapta.CreateAxaptaContainer();
            AxaptaContainer axconStatus = axapta.CreateAxaptaContainer();
            axconStatus.Clear();
            string objects = (string)axConSalesInvoiceHeader.get_Item(1);
            axconStatus.Add(objects);

            if (!response.Data.HasError)
                axconStatus.Add(1);
            else
                axconStatus.Add(-1);
            if (!string.IsNullOrEmpty(response.Data.Ettn))
                axconStatus.Add(response.Data.Ettn);
            if (!string.IsNullOrEmpty(response.Data.InvoiceNumber))
                axconStatus.Add(response.Data.InvoiceNumber);                        

            axconStatusList.Add(axconStatus);

            axapta.CallStaticClassMethod("aaEInvoice", "saveInvoiceStatus", axconStatusList);
        }
    }
}   
