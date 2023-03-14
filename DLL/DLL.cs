using IMALProcessStatement;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;
using static IMAL_FIN_Inquery.DLL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Channels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using System.Reflection;
using System.Security.Policy;
using IMAL_FIN_Inquery.Controllers;
using Azure.Core;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;

namespace IMAL_FIN_Inquery
{
    public class DLL
    {

        DAL DalCode = new DAL();
      

        string availableBalance = "";

      
        //ministatment
        string Samount = "";
        string SbriefDescriptionArab = "";
        string SbriefDescriptionEnglish = "";
        string SlongDescriptionArab = "";
        string SlongDescriptionEnglish = "";
        string SoperationNumber = "";
        string SpostDate = "";
        string StransactionBranch = "";
        string StransactionDate = "";
        string StransactionNumber = "";
        string StransactionType = "";
        string SvalueDate = "";


        //account details
        string Saddress1English = "";
        string Saddress2English = "";
        string Saddress3English = "";
        string ScountryArabic = "";
        string ScountryEnglish = "";
        string SdefaultAddress = "";
        string Semail = "";
        string Smobile = "";
        string SprintStatement = "";
        string SbriefNameArabic = "";
        string SbriefNameEnglish = "";
        string SeconomicSector = "";
        string SeconomicSectorDescription = "";
        string SjointAccount = "";
        string SlongName = "";
        string SlongNameArabic = "";
        string SaccountGl = "";
        string SaccountGlDescription = "";

        string Sbranch = "";
        string SbranchCode = "";
        string SbranchDescription = "";
        string ScifLongName = "";
        string ScifName = "";
        string ScifNo = "";
        string Scurrency = "";
        string ScurrencyDescription = "";
        string SdateSubmitted = "";
        string Sstatement = "";
 
        string Sdescription = "";
        //avaialable balance string
        string SblockedCvAmount = "";
        string SblockedFcAmount = "";

        string ScurrentMonthCvBalance = "";
        string ScurrentMonthFcBalance = "";
        string ScurrentMonthNumberCreditTransaction = "";
        string ScurrentMonthNumberDebitTransaction = "";
        string ScvAvailableBalance = "";

        string ScvBalanceLastClosedPeriod = "";
        string SfcAvailableBalance = "";
        string SnumberOnlineDebitTransactionForLastEodProcess = "";
        string SlastCreditTransactionDate = "";
        string SlastDebitTransactionDate = "";
        string SlastTransactionDate = "";
        string SnumberCreditTransactionLastClosedPeriod = "";
        string SnumberDebitTransactionLastClosedPeriod = "";
        string SnumberOfCreditTransaction = "";
        string SnumberOfDebitTransaction = "";
        string SnumberOnlineCreditTransactionForLastEodProcess = "";
    
        string SonlineCvBalanceForLastEodProcess = "";
        string SonlineFcBalanceForLastEodProcess = "";



        public string returnMiniStatment(string AdditionalRef,string username,string password,string CompanyCode,string serviceID,string serviceDomain,string operationName,string businessDomain,string businessArea,string LastN,string requesterTimeStamp,string lang,string channelname,string remoteIP)
        {
            string SstatusCode = "";
            string SstatusDesc = "";
            var SminiStatementList = "";
            lang = lang.ToUpper();
            List<Datum> datum = new List<Datum>();
            List<log> logrequest = new List<log>();
            string status = CheckChannel(channelname, username, remoteIP, "MiniStatment");
            string RequestID = "MW-MINI-" + AdditionalRef + "-" + DateTime.Now.ToString("ddMMyyyyHHmmssff");

            logrequest.Add(new log
            {

                additionalRef = AdditionalRef,
                username = username,
                password = "*******",
                lastN = LastN,
                lang = lang,
                ChannelName = channelname
            });
            string ClientRequest = JsonConvert.SerializeObject(logrequest);
            //  string  ClientRequest = JsonConvert.SerializeObject("additionalRef" + AdditionalRef + "," + " username" + username +","+ "password"+"*******"+"," + "lastN" + lastN + "," + "lang:" + lang + "," + "ChannelName" + channelname);
            DalCode.InsertLog("MiniStatment", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")), ClientRequest, "Pending", channelname, RequestID);
            if (status =="Enabled")
            {
                string GL = AdditionalRef.Substring(4, 6);

                string GLstatus = CheckStatus("MiniStatment", GL);
                if (GLstatus == "Enabled")
                {
                    HttpWebRequest request = CreateWebRequestMiniStatment();
                    XmlDocument soapEnvelopeXml = new XmlDocument();
                    soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:stat=""statementOfAccountWs"">
   <soapenv:Header/>
   <soapenv:Body>
      <stat:returnMiniStatement>
         <serviceContext>
            <businessArea>" + businessArea + @"</businessArea>
            <businessDomain>" + businessDomain + @"</businessDomain>
            <operationName>" + operationName + @"</operationName>
            <serviceDomain>" + serviceDomain + @"</serviceDomain>
            <serviceID>" + serviceID + @"</serviceID>
            <version>1.0</version>
         </serviceContext>    
         <companyCode>" + CompanyCode + @"</companyCode>  
         <account>
            <additionalRef>" + AdditionalRef + @"</additionalRef>
         </account>
         <lastN>" + LastN + @"</lastN>
         <requestContext>
         <requestID>" + RequestID + @"</requestID>
         <coreRequestTimeStamp>" + requesterTimeStamp + @"</coreRequestTimeStamp>
         </requestContext>
         <requesterContext>
            <channelID>1</channelID>
            <hashKey>1</hashKey>
              <langId>EN</langId>
            <password>" + password + @"</password>
            <requesterTimeStamp>" + requesterTimeStamp + @"</requesterTimeStamp>
            <userID>" + username + @"</userID>
         </requesterContext>
         <vendorContext>
            <license>Copyright 2018 Path Solutions. All Rights Reserved</license>
            <providerCompanyName>Path Solutions</providerCompanyName>
            <providerID>IMAL</providerID>
         </vendorContext>
      </stat:returnMiniStatement>
   </soapenv:Body>
</soapenv:Envelope>"
                        );

                    string soapResult = "";

                    using (Stream stream = request.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }


                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                            // Console.WriteLine(soapResult);
                            var str = XElement.Parse(soapResult);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(soapResult);
                            XmlNodeList elemlistCode = xmlDoc.GetElementsByTagName("statusCode");
                            SstatusCode = elemlistCode[0].InnerXml;
                            XmlNodeList elemliststatusDesc = xmlDoc.GetElementsByTagName("statusDesc");

                            SstatusDesc = elemliststatusDesc[0].InnerXml;

                            if (SstatusCode == "0")
                            {
                             
                                XmlNodeList elemlistCodeavailableBalance = xmlDoc.GetElementsByTagName("availableBalance");
                                availableBalance = elemlistCodeavailableBalance[0].InnerXml;
                         
                                XmlNodeList elemlistCodeminiStatementList = xmlDoc.GetElementsByTagName("miniStatement");
                                SminiStatementList = elemlistCodeminiStatementList[0].InnerXml;
                                foreach (XmlNode node in elemlistCodeminiStatementList)
                                {
                                    Samount = node["amount"].InnerText;

                                    if (node["briefDescriptionEnglish"] != null)
                                    {
                                        SbriefDescriptionEnglish = node["briefDescriptionEnglish"].InnerText;
                                    }
                                    if (node["briefDescriptionArab"] != null)
                                    {
                                        SbriefDescriptionArab = node["briefDescriptionArab"].InnerText;
                                    }
                                    if (node["longDescriptionArab"] != null)
                                    {
                                        SlongDescriptionArab = node["longDescriptionArab"].InnerText;
                                    }
                                    if (node["longDescriptionEnglish"] != null)
                                    {
                                        SlongDescriptionEnglish = node["longDescriptionEnglish"].InnerText;
                                    }
                                    Sdescription = node["description"].InnerText;
                                    SoperationNumber = node["operationNumber"].InnerText;
                                    SpostDate = node["postDate"].InnerText;
                                    StransactionBranch = node["transactionBranch"].InnerText;
                                    StransactionDate = node["transactionDate"].InnerText;
                                    StransactionNumber = node["transactionNumber"].InnerText;
                                    StransactionType = node["transactionType"].InnerText;
                                    if (StransactionType == "C")
                                    {
                                        StransactionType = "Credit";

                                    }
                                    else
                                    {
                                        if (StransactionType == "D")
                                        {
                                            StransactionType = "Debit";
                                        }
                                    }
                                    SvalueDate = node["valueDate"].InnerText;

                             
                                    if (lang == "EN")
                                    {
                                        CheckFields(channelname, "MiniStatment");
                                        datum.Add(new Datum
                                        {
                                            amount = Samount,
                                            briefDescription = SbriefDescriptionEnglish,
                                            description = Sdescription,
                                            longDescription = SlongDescriptionEnglish,
                                            operationNumber = SoperationNumber,
                                            postDate = SpostDate,
                                            transactionBranch = StransactionBranch,
                                            transactionDate = StransactionDate,
                                            transactionNumber = StransactionNumber,
                                            transactionType = StransactionType,
                                            valueDate = SvalueDate,
                                            statusCode = SstatusCode,
                                            statusDesc = SstatusDesc


                                        });
                                    }
                                    else
                                    {
                                        if (lang == "AR")
                                        {
                                            CheckFields(channelname, "MiniStatment");
                                            datum.Add(new Datum
                                            {
                                                amount = Samount,
                                                briefDescription = SbriefDescriptionArab,
                                                description = Sdescription,
                                                longDescription = SlongDescriptionArab,
                                                operationNumber = SoperationNumber,
                                                postDate = SpostDate,
                                                transactionBranch = StransactionBranch,
                                                transactionDate = StransactionDate,
                                                transactionNumber = StransactionNumber,
                                                transactionType = StransactionType,
                                                valueDate = SvalueDate,
                                                statusCode = SstatusCode,
                                                statusDesc = SstatusDesc
                                            });
                                        }
                                    }


                                }

                            }
                            else
                            {
                                XmlNodeList elemliststatusCode = xmlDoc.GetElementsByTagName("statusCode");
                                SstatusCode = elemliststatusCode[0].InnerXml;
                                XmlNodeList elemlistDesc = xmlDoc.GetElementsByTagName("statusDesc");
                                SstatusDesc = elemlistDesc[0].InnerXml;

                                datum.Add(new Datum
                                {
                                    amount = Samount,

                                    description = Sdescription,

                                    operationNumber = SoperationNumber,
                                    postDate = SpostDate,
                                    transactionBranch = StransactionBranch,
                                    transactionDate = StransactionDate,
                                    transactionNumber = StransactionNumber,
                                    transactionType = StransactionType,
                                    valueDate = SvalueDate,
                                    statusCode = SstatusCode,
                                    statusDesc = SstatusDesc
                                });


                            }
                        }
                    }
                }else
                {
                    SstatusCode = "-986";
                    SstatusDesc = "GL Not Allowed";
                    datum.Add(new Datum
                    {
                      
                        statusCode = SstatusCode,
                        statusDesc = SstatusDesc
                    });
                }
            }
            else
            {
                SstatusCode = "-985";
                SstatusDesc = "you are not authorize";
                datum.Add(new Datum
                {
                  
                    statusCode = SstatusCode,
                    statusDesc = SstatusDesc
                });
            }
            string statuslog = "";
            if(SstatusCode =="0")
            {
                statuslog = "Success";
            }
            else
            {
                statuslog = "Failed";
            }
            DalCode.UpdateLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), JsonConvert.SerializeObject((datum)), statuslog,channelname,RequestID);
            return JsonConvert.SerializeObject((datum));
        }
        public class log
        {
            public string? additionalRef { get; set; }
            public string? username { get; set; }
            public string? password { get; set; }
            public string? lastN { get; set; }

            public string? lang { get; set; }

            public string? ChannelName { get; set; }

        }

        public class ReqAvilbal
        {
            public string? additionalRef { get; set; }
            public string? username { get; set; }
            public string? password { get; set; }

            public string? ChannelName { get; set; }

        }

        public class RespAvilb
        {
            public string? blockedCvAmount { get; set; }

            public string? blockedFcAmount { get; set; }

            public string? currentMonthCvBalance { get; set; }  

            public string? currentMonthFcBalance { get; set; }

            public string? currentMonthNumberCreditTransaction { get; set; }

            public string? currentMonthNumberDebitTransaction { get; set; }

            public string? cvAvailableBalance { get; set; } 

            public string? cvBalanceLastClosedPeriod { get; set; }

            public string? fcAvailableBalance { get; set; }

            public string? lastCreditTransactionDate { get; set; }
            public string? lastDebitTransactionDate { get; set; }

            public string? lastTransactionDate { get; set; }

            public string? numberCreditTransactionLastClosedPeriod { get; set; }
            public string? numberDebitTransactionLastClosedPeriod { get; set; }
            public string? numberOfCreditTransaction { get; set; }
            public string? numberOfDebitTransaction { get; set; }
            public string? numberOnlineCreditTransactionForLastEodProcess { get; set; } 

            public string? numberOnlineDebitTransactionForLastEodProcess { get; set; }

            public string? onlineCvBalanceForLastEodProcess { get; set; }

            public string? onlineFcBalanceForLastEodProcess { get; set; }

            public string? AvailableAmount { get; set; }

            public string? statusCode { get; set; }

            public string? statusDesc { get; set; }


        }

        public class ReqACCDT
        {
            public string? additionalRef { get; set; }
            public string? username { get; set; }
            public string? password { get; set; }

            public string? ChannelName { get; set; }

        }
        public class Datum
        {
    
            public string? amount { get; set; }

            public string? briefDescription { get; set; }

        

            public string? description { get; set; }
            public string? longDescription { get; set; }
   
            public string? operationNumber { get; set; }

            public string? postDate { get; set; }
            public string? transactionBranch { get; set; }
            public string? transactionDate { get; set; }
            public string? transactionNumber { get; set; }
            public string? transactionType { get; set; }
            public string? valueDate { get; set; }

            public string? statusCode { get; set; }

            public string? statusDesc { get; set; }






        }

        public class RespACCT
        {
    




            public string? address1English { get; set; }
            public string? address2English { get; set; }
            public string? address3English { get; set; }
            public string? countryArabic { get; set; }
            public string? countryEnglish { get; set; }
            public string? defaultAddress { get; set; }
            public string? email { get; set; }
            public string? mobile { get; set; }
            public string? printStatement { get; set; }
            public string? briefNameArabic { get; set; }
            public string? briefNameEnglish { get; set; }
            public string? economicSector { get; set; }
            public string? economicSectorDescription { get; set; }
            public string? jointAccount { get; set; }
            public string? longName { get; set; }
            public string? longNameArabic { get; set; }
            public string? accountGl { get; set; }
            public string? accountGlDescription { get; set; }

            public string? branch  { get; set; }
        public string? branchCode { get; set; }
        public string? branchDescription { get; set; }
        public string? cifLongName { get; set; }
        public string? cifName { get; set; }
        public string? cifNo { get; set; }
            public string? currency { get; set; }
            public string? currencyDescription { get; set; }
            public string?  dateSubmitted { get; set; }
            public string? statement { get; set; }

            public string? statusCode { get; set; } 

            public string? statusDesc { get; set; } 

        }
     
        public static HttpWebRequest CreateWebRequestMiniStatment()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var URLMiniStatment = MyConfig.GetValue<string>("AppSettings:URLMiniStatment");
       
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URLMiniStatment);
            webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static HttpWebRequest CreateWebRequestAvilBalance()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var URLAvilBalance = MyConfig.GetValue<string>("AppSettings:URLAvilBalance");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URLAvilBalance);
            webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static HttpWebRequest CreateWebRequestGeneralAccount()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var URLGeneralaccount = MyConfig.GetValue<string>("AppSettings:URLGeneralaccount");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URLGeneralaccount);
            webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public void CheckFields(string ChannelName,string @ServiceName)
        {
            string FieldStatus = "";
            string FieldName = "";
            DataTable dt_CheckFields = DalCode.IMALResponseField(ServiceName,ChannelName);
            DLL[] BR_CheckFields = new DLL[dt_CheckFields.Rows.Count];


            if (BR_CheckFields.Length != 0)
            {
                int ii;
                for (ii = 0; ii < dt_CheckFields.Rows.Count; ii++)
                {

                    FieldStatus = dt_CheckFields.Rows[ii]["FieldStatus"].ToString().Trim();
                    FieldName = dt_CheckFields.Rows[ii]["FieldName"].ToString().Trim();
                    if((FieldStatus == "0") && (FieldName == "amount"))
                    {
                        Samount = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "description"))
                    {
                        Sdescription = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "longDescription"))
                    {
                        SlongDescriptionEnglish = "";
                        SlongDescriptionArab = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "briefDescription"))
                    {
                        SbriefDescriptionArab = "";
                        SbriefDescriptionEnglish = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "operationNumber"))
                    {
                        SoperationNumber = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "postDate"))
                    {
                        SpostDate = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "transactionBranch"))
                    {
                        StransactionBranch = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "transactionDate"))
                    {
                        StransactionDate = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "transactionNumber"))
                    {
                        StransactionNumber = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "transactionType"))
                    {
                        StransactionType = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "valueDate"))
                    {
                        SvalueDate = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "valueDate"))
                    {
                        SvalueDate = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "address1English"))
                    {
                        Saddress1English = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "address2English"))
                    {
                        Saddress2English = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "address3English"))
                    {
                        Saddress3English = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "countryArabic"))
                    {
                        ScountryArabic = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "countryEnglish"))
                    {
                        ScountryEnglish = "";

                    }
                    if ((FieldStatus == "0") && (FieldName == "email"))
                    {
                        Semail = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "defaultAddress"))
                    {
                        SdefaultAddress = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "mobile"))
                    {
                        Smobile = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "printStatement"))
                    {
                        SprintStatement = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "briefNameArabic"))
                    {
                        SbriefNameArabic = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "briefNameEnglish"))
                    {
                        SbriefNameEnglish = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "economicSector"))
                    {
                        SeconomicSector = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "economicSectorDescription"))
                    {
                        SeconomicSectorDescription = "";
                    }
                    if ((FieldStatus == "0") && (FieldName == "jointAccount"))
                    {
                        SjointAccount = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "longName"))
                    {
                        SlongName = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "longNameArabic"))
                    {
                        SlongNameArabic = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "accountGl"))
                    {
                        SaccountGl = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "accountGlDescription"))
                    {
                        SaccountGlDescription = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "branch"))
                    {
                        Sbranch = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "branchCode"))
                    {
                        SbranchCode = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "branchDescription"))
                    {
                        SbranchDescription = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "cifLongName"))
                    {
                        ScifLongName = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "cifName"))
                    {
                        ScifName = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "cifNo"))
                    {
                        ScifNo = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "currency"))
                    {
                        Scurrency = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "currencyDescription"))
                    {
                        ScurrencyDescription = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "dateSubmitted"))
                    {
                        SdateSubmitted = "";
                    }

                    if ((FieldStatus == "0") && (FieldName == "statement"))
                    {
                        Sstatement = "";
                    }

                   
                }
            }
        }

        public string CheckChannel(String ChannelName,string username,string ChannelIP,string ServiceName)
        {

            string statusChannel = "";
            string EnableChannel = "";
            DataTable dt_Channel = DalCode.IMALChannelstatus(ChannelName,username,ChannelIP,ServiceName);
            DLL[] BR_Channel = new DLL[dt_Channel.Rows.Count];


            if (BR_Channel.Length != 0)
            {
                int ii;
                for (ii = 0; ii < dt_Channel.Rows.Count; ii++)
                {
           
                    EnableChannel = dt_Channel.Rows[ii]["EnableChannel"].ToString().Trim();
                }
            }
            if(EnableChannel =="1")
            {
                statusChannel = "Enabled";
            }
            else
            {
                statusChannel = "Disabled";
            }
            return statusChannel;
        }

        public string CheckStatus(string ServiceName, string GLNo)
        {

            string statusGL = "";
            string EnableGL = "";
            DataTable dt_GL = DalCode.IMALGLStatus(GLNo,ServiceName);
            DLL[] BR_GL = new DLL[dt_GL.Rows.Count];


            if (BR_GL.Length != 0)
            {
                int ii;
                for (ii = 0; ii < dt_GL.Rows.Count; ii++)
                {

                    EnableGL = dt_GL.Rows[ii]["EnableGL"].ToString().Trim();
                }
            }
            if (EnableGL == "1")
            {
                statusGL = "Enabled";
            }
            else
            {
                statusGL = "Disabled";
            }
            return statusGL;
        }

        public string  ReturnAvailBalance(string AdditionalRef,string channelname,string username,string password,string remoteIP,string businessArea,string businessDomain,string operationName,string serviceDomain,string serviceID,string companyCode,string requesterTimeStamp)
        {
         
            string SstatusCode = "";
            string SstatusDesc = "";
            string RequestID = "MW-AVBAL-" + AdditionalRef + "-" + DateTime.Now.ToString("ddMMyyyyHHmmssff");
            List<ReqAvilbal> logrequest = new List<ReqAvilbal>();
            List<RespAvilb> respnseavail = new List<RespAvilb>();
            string status = CheckChannel(channelname, username, remoteIP, "AvailBalance");
            logrequest.Add(new ReqAvilbal
            {

                additionalRef = AdditionalRef,
                username = username,
                password = "*******",
                ChannelName = channelname
            });
            string ClientRequest = JsonConvert.SerializeObject(logrequest);
            if (status == "Enabled")
            {
                string GL = AdditionalRef.Substring(4, 6);

                string GLstatus = CheckStatus("AvailBalance", GL);
                if (GLstatus == "Enabled")
                {
                    HttpWebRequest request = CreateWebRequestAvilBalance();
                    XmlDocument soapEnvelopeXml = new XmlDocument();
                    soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:stat=""statementOfAccountWs"">
   <soapenv:Header/>
   <soapenv:Body>
      <stat:returnAvailableBalance>
         <serviceContext>  
      <businessArea>" + businessArea + @"</businessArea>
            <businessDomain>" + businessDomain + @"</businessDomain>
            <operationName>" + operationName + @"</operationName>
            <serviceDomain>" + serviceDomain + @"</serviceDomain>
            <serviceID>" + serviceID + @"</serviceID>
            <version>1.0</version>
         </serviceContext>         
         <companyCode>" + companyCode + @"</companyCode>
         <account>
            <additionalRef>" + AdditionalRef + @"</additionalRef>
         </account>       
         <requestContext>
            <requestID>" + RequestID + @"</requestID>
            <coreRequestTimeStamp>" + requesterTimeStamp + @"</coreRequestTimeStamp>
             <!--<requestKernelDetails>?</requestKernelDetails>-->
         </requestContext>
         <requesterContext>
            <channelID>1</channelID>
            <hashKey>1</hashKey>
              <langId>EN</langId>
            <password>" + password + @"</password>
            <requesterTimeStamp>" + requesterTimeStamp + @"</requesterTimeStamp>
            <userID>" + username + @"</userID>
         </requesterContext>
         <vendorContext>
            <license>Copyright 2018 Path Solutions. All Rights Reserved</license>
            <providerCompanyName>Path Solutions</providerCompanyName>
            <providerID>IMAL</providerID>
         </vendorContext>
      </stat:returnAvailableBalance>
   </soapenv:Body>
</soapenv:Envelope>"
                         );

                    string soapResult = "";

                    using (Stream stream = request.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            soapResult = rd.ReadToEnd();
                            // Console.WriteLine(soapResult);
                            var str = XElement.Parse(soapResult);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(soapResult);
                            XmlNodeList elemlistCode = xmlDoc.GetElementsByTagName("statusCode");
                            SstatusCode = elemlistCode[0].InnerXml;
                            XmlNodeList elemliststatusDesc = xmlDoc.GetElementsByTagName("statusDesc");

                            SstatusDesc = elemliststatusDesc[0].InnerXml;

                            if (SstatusCode == "0")
                            {
                             foreach (XmlNode node in xmlDoc)
                                {
                                    XmlNodeList ESblockedCvAmount = xmlDoc.GetElementsByTagName("blockedCvAmount");
                                    SblockedCvAmount = ESblockedCvAmount[0].InnerXml;
                                    XmlNodeList EblockedFcAmount = xmlDoc.GetElementsByTagName("blockedFcAmount");
                                    SblockedFcAmount = EblockedFcAmount[0].InnerXml;
                                    XmlNodeList EScurrentMonthCvBalance = xmlDoc.GetElementsByTagName("currentMonthCvBalance");
                                    ScurrentMonthCvBalance = EScurrentMonthCvBalance[0].InnerXml;
                                    XmlNodeList EScurrentMonthFcBalance = xmlDoc.GetElementsByTagName("currentMonthFcBalance");
                                    ScurrentMonthFcBalance = EScurrentMonthFcBalance[0].InnerXml;
                                    XmlNodeList EScurrentMonthNumberCreditTransaction = xmlDoc.GetElementsByTagName("currentMonthNumberCreditTransaction");
                                    ScurrentMonthNumberCreditTransaction = EScurrentMonthNumberCreditTransaction[0].InnerXml;
                                    XmlNodeList EScurrentMonthNumberDebitTransaction = xmlDoc.GetElementsByTagName("currentMonthNumberDebitTransaction");
                                    ScurrentMonthNumberDebitTransaction = EScurrentMonthNumberDebitTransaction[0].InnerXml;
                                    XmlNodeList EScvAvailableBalance = xmlDoc.GetElementsByTagName("cvAvailableBalance");
                                    ScvAvailableBalance = EScvAvailableBalance[0].InnerXml;
                                    XmlNodeList EScvBalanceLastClosedPeriod = xmlDoc.GetElementsByTagName("cvBalanceLastClosedPeriod");
                                    ScvBalanceLastClosedPeriod = EScvBalanceLastClosedPeriod[0].InnerXml;
                                    XmlNodeList ESfcAvailableBalance = xmlDoc.GetElementsByTagName("fcAvailableBalance");
                                    SfcAvailableBalance = ESfcAvailableBalance[0].InnerXml;
                                    XmlNodeList ESlastCreditTransactionDate = xmlDoc.GetElementsByTagName("lastCreditTransactionDate");
                                    SlastCreditTransactionDate = ESlastCreditTransactionDate[0].InnerXml;
                                    XmlNodeList ESlastDebitTransactionDate = xmlDoc.GetElementsByTagName("lastDebitTransactionDate");
                                    SlastDebitTransactionDate = ESlastDebitTransactionDate[0].InnerXml;
                                    XmlNodeList ESlastTransactionDate = xmlDoc.GetElementsByTagName("lastTransactionDate");
                                    SlastTransactionDate = ESlastTransactionDate[0].InnerXml;
                                    XmlNodeList ESnumberCreditTransactionLastClosedPeriod = xmlDoc.GetElementsByTagName("numberCreditTransactionLastClosedPeriod");
                                    SnumberCreditTransactionLastClosedPeriod = ESnumberCreditTransactionLastClosedPeriod[0].InnerXml;
                                    XmlNodeList ESnumberDebitTransactionLastClosedPeriod = xmlDoc.GetElementsByTagName("numberDebitTransactionLastClosedPeriod");
                                    SnumberDebitTransactionLastClosedPeriod = ESnumberDebitTransactionLastClosedPeriod[0].InnerXml;
                                    XmlNodeList ESnumberOfCreditTransaction = xmlDoc.GetElementsByTagName("numberOfCreditTransaction");
                                    SnumberOfCreditTransaction = ESnumberOfCreditTransaction[0].InnerXml;
                                    XmlNodeList ESnumberOfDebitTransaction = xmlDoc.GetElementsByTagName("numberOfDebitTransaction");
                                    SnumberOfDebitTransaction = ESnumberOfDebitTransaction[0].InnerXml;
                                    XmlNodeList ESnumberOnlineCreditTransactionForLastEodProcess = xmlDoc.GetElementsByTagName("numberOnlineCreditTransactionForLastEodProcess");
                                    SnumberOnlineCreditTransactionForLastEodProcess = ESnumberOnlineCreditTransactionForLastEodProcess[0].InnerXml;
                                    XmlNodeList EnumberOnlineDebitTransactionForLastEodProcess = xmlDoc.GetElementsByTagName("numberOnlineDebitTransactionForLastEodProcess");
                                    SnumberOnlineDebitTransactionForLastEodProcess = EnumberOnlineDebitTransactionForLastEodProcess[0].InnerXml;
                                    XmlNodeList EonlineCvBalanceForLastEodProcess = xmlDoc.GetElementsByTagName("onlineCvBalanceForLastEodProcess");
                                    SonlineCvBalanceForLastEodProcess = EonlineCvBalanceForLastEodProcess[0].InnerXml;
                                    XmlNodeList EonlineFcBalanceForLastEodProcess = xmlDoc.GetElementsByTagName("onlineFcBalanceForLastEodProcess");
                                    SonlineFcBalanceForLastEodProcess = EonlineFcBalanceForLastEodProcess[0].InnerXml;

                    
                              
                                }
                            }
                        }
                    }
                    var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var CompanyCodeACC = MyConfig.GetValue<string>("AppSettings:CompanyCode");
                    var serviceIDACC = MyConfig.GetValue<string>("AppSettings:serviceIDAccD");
                    var serviceDomainACC = MyConfig.GetValue<string>("AppSettings:serviceDomainAccID");
                    var operationNameACC = MyConfig.GetValue<string>("AppSettings:operationNameAccID");
                    var businessDomainACC = MyConfig.GetValue<string>("AppSettings:businessDomain");
                    var businessAreaACC = MyConfig.GetValue<string>("AppSettings:businessArea");

                    string avail = "";
                    string result = ReturnGetaccountDetails(AdditionalRef, channelname, username, remoteIP, businessAreaACC, businessDomainACC, operationNameACC, serviceDomainACC, serviceIDACC, CompanyCodeACC, System.DateTime.Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss"), password);
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    string currency = data["currency"].Value<string>();
                        if(currency =="818")
                    {
                        decimal cvAvailableBalanceNO = Convert.ToDecimal(ScvAvailableBalance) + Convert.ToDecimal(SblockedCvAmount);
                         avail = Convert.ToString(cvAvailableBalanceNO);
                        string chars = avail.Substring(0, 1);
                        if (chars == "-")
                        {
                            avail = avail.Remove(0, 1);
                        }
                        else
                        {
                            avail = "-" + avail;
                        }

                    }
                        else
                    {
                        decimal fcAvailableBalanceInt = Convert.ToDecimal(SfcAvailableBalance) + Convert.ToDecimal(SblockedFcAmount);
                         avail = Convert.ToString(fcAvailableBalanceInt);
                        string chars = avail.Substring(0, 1);
                        if (chars == "-")
                        {
                            avail = avail.Remove(0, 1);
                        }
                        else
                        {
                            avail = "-" + avail;
                        }
                    }
                    CheckFields(channelname, "ACCDetails");
                    respnseavail.Add(new RespAvilb
                    {
                        blockedCvAmount = SblockedCvAmount,
                        blockedFcAmount = SblockedFcAmount,
                        currentMonthCvBalance = ScurrentMonthCvBalance,
                        currentMonthFcBalance = ScurrentMonthFcBalance,
                        currentMonthNumberCreditTransaction = ScurrentMonthNumberCreditTransaction,
                        currentMonthNumberDebitTransaction = ScurrentMonthNumberDebitTransaction,
                        cvAvailableBalance = ScvAvailableBalance,
                        cvBalanceLastClosedPeriod = ScvBalanceLastClosedPeriod,
                        fcAvailableBalance = SfcAvailableBalance,
                        lastCreditTransactionDate = SlastCreditTransactionDate,
                        lastDebitTransactionDate = SlastDebitTransactionDate,
                        lastTransactionDate = SlastTransactionDate,
                        numberCreditTransactionLastClosedPeriod = SnumberCreditTransactionLastClosedPeriod,
                        numberDebitTransactionLastClosedPeriod = SnumberDebitTransactionLastClosedPeriod,
                        numberOfCreditTransaction = SnumberOfCreditTransaction,
                        numberOfDebitTransaction = SnumberOfDebitTransaction,
                        onlineCvBalanceForLastEodProcess = SonlineCvBalanceForLastEodProcess,
                        onlineFcBalanceForLastEodProcess = SonlineFcBalanceForLastEodProcess,
                        numberOnlineCreditTransactionForLastEodProcess = SnumberOnlineCreditTransactionForLastEodProcess,
                        numberOnlineDebitTransactionForLastEodProcess = SnumberOnlineDebitTransactionForLastEodProcess,
                        AvailableAmount = avail,
                        statusCode = SstatusCode,
                        statusDesc = SstatusDesc



                    }) ;
            
                }
                
                else
                {
                    SstatusCode = "-985";
                    SstatusDesc = "you are not authorize";
                    respnseavail.Add(new RespAvilb
                    {

                        statusCode = SstatusCode,
                        statusDesc = SstatusDesc
                    });
                }
            }
            string statuslog = "";
            if (SstatusCode == "0")
            {
                statuslog = "Success";
            }
            else
            {
                statuslog = "Failed";
            }
            DalCode.UpdateLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), JsonConvert.SerializeObject((respnseavail)), statuslog, channelname, RequestID);
            return Convert.ToString(availableBalance);
        }


       

        public string ReturnGetaccountDetails(string AdditionalRef, string channelname, string username, string remoteIP,string businessArea,string businessDomain,string operationName,string serviceDomain,string serviceID,string companyCode,string requesterTimeStamp,string Password)
        {
            string SstatusCode = "";
            string SstatusDesc = "";

        
            List<RespACCT> datum = new List<RespACCT>();
            string RequestID = "MW-ACCDT-" + AdditionalRef + "-" + DateTime.Now.ToString("ddMMyyyyHHmmssff");
            List<ReqACCDT> logrequest = new List<ReqACCDT>();
            string status = CheckChannel(channelname, username, remoteIP, "ACCDetails");
            logrequest.Add(new ReqACCDT
            {

                additionalRef = AdditionalRef,
                username = username,
                password = "*******",
                ChannelName = channelname
            });
            string ClientRequest = JsonConvert.SerializeObject(logrequest);
            DalCode.InsertLog("ACCDetails", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")), ClientRequest, "Pending", channelname, RequestID);
            if (status == "Enabled")
            {
                HttpWebRequest request = CreateWebRequestGeneralAccount();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:gen=""generalAccountsWs"">
   <soapenv:Header/>
   <soapenv:Body>
      <gen:returnGeneralAccountDetails>
         <serviceContext>
            <businessArea>"+ businessArea + @"</businessArea>
            <businessDomain>"+ businessDomain + @"</businessDomain>
            <operationName>"+ operationName + @"</operationName>
            <serviceDomain>"+ serviceDomain + @"</serviceDomain>
            <serviceID>"+ serviceID + @"</serviceID>
            <version>1.0</version>
         </serviceContext>
         <companyCode>"+ companyCode + @"</companyCode>
         <branchCode>5599</branchCode>
         <account>
            <additionalRef>"+AdditionalRef+@"</additionalRef>
         </account>
          <showAccountHistoryDetails>0</showAccountHistoryDetails>
         <showAdditionalFields>0</showAdditionalFields>
         <showChargesDetails>0</showChargesDetails>
         
          <requestContext>
           <requestID>"+RequestID+@"</requestID>
        <coreRequestTimeStamp>"+ requesterTimeStamp + @"</coreRequestTimeStamp>
         </requestContext>
         <requesterContext>
            <channelID>1</channelID> 
            <hashKey>1</hashKey> 
              <langId>EN</langId> 
            <password>"+Password+@"</password> 
            <requesterTimeStamp>"+ requesterTimeStamp + @"</requesterTimeStamp> 
            <userID>"+username+@"</userID> 
         </requesterContext>
         <vendorContext>
            <license>Copyright 2018 Path Solutions. All Rights Reserved</license>   
            <providerCompanyName>Path Solutions</providerCompanyName>
            <providerID>IMAL</providerID>
         </vendorContext>
      </gen:returnGeneralAccountDetails>
   </soapenv:Body>
</soapenv:Envelope>"
                    );
                string soapResult = "";

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        // Console.WriteLine(soapResult);
                        var str = XElement.Parse(soapResult);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(soapResult);
                        XmlNodeList elemlistCode = xmlDoc.GetElementsByTagName("statusCode");
                        SstatusCode = elemlistCode[0].InnerXml;
                        XmlNodeList elemliststatusDesc = xmlDoc.GetElementsByTagName("statusDesc");

                        SstatusDesc = elemliststatusDesc[0].InnerXml;

                        if (SstatusCode == "0")
                        {
                            foreach (XmlNode node in xmlDoc)
                            {

                          
                                    XmlNodeList elemlistaddress1English = xmlDoc.GetElementsByTagName("address1English");
                                Saddress1English = elemlistaddress1English[0].InnerXml;
                                XmlNodeList elemlistaccountGl = xmlDoc.GetElementsByTagName("accountGl");
                                SaccountGl = elemlistaccountGl[0].InnerXml;
                                XmlNodeList elemlistaccountGlDescription = xmlDoc.GetElementsByTagName("accountGlDescription");
                                SaccountGlDescription = elemlistaccountGlDescription[0].InnerXml;
                    
                                XmlNodeList elemlistaddress2English = xmlDoc.GetElementsByTagName("address2English");
                                Saddress2English = elemlistaddress2English[0].InnerXml;
                                XmlNodeList elemlistaddress3English = xmlDoc.GetElementsByTagName("address3English");
                                Saddress3English = elemlistaddress3English[0].InnerXml;
                                XmlNodeList elemlistcountryArabic = xmlDoc.GetElementsByTagName("countryArabic");
                                ScountryArabic = elemlistcountryArabic[0].InnerXml;
                                XmlNodeList elemlistcountryEnglish = xmlDoc.GetElementsByTagName("countryEnglish");
                                ScountryEnglish = elemlistcountryEnglish[0].InnerXml;
                                XmlNodeList elemlistemail = xmlDoc.GetElementsByTagName("email");
                                Semail = elemlistemail[0].InnerXml;
                                XmlNodeList elemlistmobile = xmlDoc.GetElementsByTagName("mobile");
                                Smobile = elemlistmobile[0].InnerXml;
                                XmlNodeList elemlistdefaultAddress = xmlDoc.GetElementsByTagName("defaultAddress");
                                SdefaultAddress = elemlistdefaultAddress[0].InnerXml;
                                XmlNodeList elemlistprintStatement = xmlDoc.GetElementsByTagName("printStatement");
                                SprintStatement = elemlistprintStatement[0].InnerXml;
                                XmlNodeList elemlistbriefNameArabic = xmlDoc.GetElementsByTagName("briefNameArabic");
                                SbriefNameArabic = elemlistbriefNameArabic[0].InnerXml;
                                XmlNodeList elemlistbriefNameEnglish = xmlDoc.GetElementsByTagName("briefNameEnglish");
                                SbriefNameEnglish = elemlistbriefNameEnglish[0].InnerXml;
                                XmlNodeList elemlisteconomicSector = xmlDoc.GetElementsByTagName("economicSector");
                                SeconomicSector = elemlisteconomicSector[0].InnerXml;
                                XmlNodeList elemlisteconomicSectorDescription = xmlDoc.GetElementsByTagName("economicSectorDescription");
                                SeconomicSectorDescription = elemlisteconomicSectorDescription[0].InnerXml;
                                XmlNodeList elemlistjointAccount = xmlDoc.GetElementsByTagName("jointAccount");
                                SjointAccount = elemlistjointAccount[0].InnerXml;
                                XmlNodeList elemlistlongName = xmlDoc.GetElementsByTagName("longName");
                                SlongName = elemlistlongName[0].InnerXml;
                                XmlNodeList elemlistlongNameArabic = xmlDoc.GetElementsByTagName("longNameArabic");
                                SlongNameArabic = elemlistlongNameArabic[0].InnerXml;
                                SaccountGlDescription = elemlistaccountGlDescription[0].InnerXml;
                                XmlNodeList elemlistbranch = xmlDoc.GetElementsByTagName("branch");
                                Sbranch = elemlistbranch[0].InnerXml;
                                XmlNodeList elemlistbranchCode = xmlDoc.GetElementsByTagName("branchCode");
                                SbranchCode = elemlistbranchCode[0].InnerXml;
                                XmlNodeList elemlistbranchDescription = xmlDoc.GetElementsByTagName("branchDescription");
                                SbranchDescription = elemlistbranchDescription[0].InnerXml; ;
                                XmlNodeList elemlistcifLongName = xmlDoc.GetElementsByTagName("cifLongName");
                                ScifLongName = elemlistcifLongName[0].InnerXml;
                                XmlNodeList elemlistcifName = xmlDoc.GetElementsByTagName("cifName");
                                ScifName = elemlistcifName[0].InnerXml;
                                XmlNodeList elemlistcifNo = xmlDoc.GetElementsByTagName("cifNo");
                                ScifNo = elemlistcifNo[0].InnerXml;
                                XmlNodeList elemlistcurrency = xmlDoc.GetElementsByTagName("currency");
                                Scurrency = elemlistcurrency[0].InnerXml;
                                XmlNodeList elemlistcurrencyDescription = xmlDoc.GetElementsByTagName("currencyDescription");
                                ScurrencyDescription = elemlistcurrencyDescription[0].InnerXml;
                                XmlNodeList elemlistdateSubmitted = xmlDoc.GetElementsByTagName("dateSubmitted");
                                SdateSubmitted = elemlistdateSubmitted[0].InnerXml;
                                XmlNodeList elemliststatement = xmlDoc.GetElementsByTagName("statement");
                                Sstatement = elemliststatement[0].InnerXml;

                                CheckFields(channelname, "ACCDetails");
                                datum.Add(new RespACCT
                                {


                                    address1English = Saddress1English,
                                address2English = Saddress2English,
                                address3English = Saddress3English,
                                countryArabic = ScountryArabic,
                                    countryEnglish =ScountryEnglish,
                                    defaultAddress = SdefaultAddress,
                                    email = Semail,
                                    mobile = Smobile,
                                    printStatement = SprintStatement,
                                    briefNameArabic = SbriefNameArabic,
                                    briefNameEnglish = SbriefNameEnglish,
                                    economicSector = SeconomicSector,
                                    economicSectorDescription =SeconomicSectorDescription,
                                    jointAccount = SjointAccount,
                                    longName = SlongName,
                                    longNameArabic = SlongNameArabic,
                                    accountGl = SaccountGl,
                                    accountGlDescription = SaccountGlDescription,
                                    branch = Sbranch,
                                    branchCode = SbranchCode,
                                    branchDescription = SbranchDescription,
                                    cifLongName = ScifLongName,
                                    cifName = ScifName,
                                    cifNo = ScifNo,
                                    currency=Scurrency,
                                    currencyDescription = ScurrencyDescription,
                                    dateSubmitted =SdateSubmitted,
                                    statement = Sstatement,
                                    statusCode = SstatusCode,
                                    statusDesc = SstatusDesc
                                });

                                }

                        }
                    }
                }
            }
            else
            {
                SstatusCode = "-985";
                SstatusDesc = "you are not authorize";
                datum.Add(new RespACCT
                {

                    statusCode = SstatusCode,
                    statusDesc = SstatusDesc
                });
            }
            string statuslog = "";
            if (SstatusCode == "0")
            {
                statuslog = "Success";
            }
            else
            {
                statuslog = "Failed";
            }
            DalCode.UpdateLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), JsonConvert.SerializeObject((datum)), statuslog, channelname, RequestID);
            return JsonConvert.SerializeObject((datum));

        }

    }
}
