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

namespace IMAL_FIN_Inquery
{
    public class DLL
    {

 

        public string returnMiniStatment(string AdditionalRef,string username,string password,string CompanyCode,string serviceID,string serviceDomain,string operationName,string businessDomain,string businessArea,string LastN,string requesterTimeStamp,string lang)
        {
    
            HttpWebRequest request = CreateWebRequestMiniStatment();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:stat=""statementOfAccountWs"">
   <soapenv:Header/>
   <soapenv:Body>
      <stat:returnMiniStatement>
         <serviceContext>
            <businessArea>"+ businessArea + @"</businessArea>
            <businessDomain>"+ businessDomain + @"</businessDomain>
            <operationName>"+ operationName + @"</operationName>
            <serviceDomain>"+serviceDomain+@"</serviceDomain>
            <serviceID>"+ serviceID + @"</serviceID>
            <version>1.0</version>
         </serviceContext>    
         <companyCode>"+ CompanyCode + @"</companyCode>  
         <account>
            <additionalRef>"+ AdditionalRef + @"</additionalRef>
         </account>
         <lastN>"+ LastN + @"</lastN>
         <requestContext>
         </requestContext>
         <requesterContext>
            <channelID>1</channelID>
            <hashKey>1</hashKey>
              <langId>EN</langId>
            <password>" + password + @"</password>
            <requesterTimeStamp>"+ requesterTimeStamp + @"</requesterTimeStamp>
            <userID>"+username+@"</userID>
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
            string statusCode = "";
            string statusDesc = "";
            string Error = "";
            string availableBalance = "";
            int lastN = 0;
       
            var amount = "";
            var briefDescriptionArab = "";
            var briefDescriptionEnglish = "";
            var description = "";
            var longDescriptionArab = "";
            var longDescriptionEnglish = "";
            var operationNumber = "";
            var postDate = "";
            var transactionBranch = "";
            var transactionDate = "";
            var transactionNumber = "";
            var transactionType = "";
            var valueDate = "";
            var miniStatementList = "";
            lang = lang.ToUpper();
            List<Datum> datum = new List<Datum>();

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
                    statusCode = elemlistCode[0].InnerXml;
                    XmlNodeList elemliststatusDesc = xmlDoc.GetElementsByTagName("statusDesc");

                    statusDesc = elemliststatusDesc[0].InnerXml;

                    if (statusCode == "0")
                    {
                        XmlNodeList elemlistCodebriefDescriptionArab = null;
                        XmlNodeList elemlistCodeavailableBalance = xmlDoc.GetElementsByTagName("availableBalance");
                        availableBalance = elemlistCodeavailableBalance[0].InnerXml;
                        XmlNodeList elemlistCodelastN = xmlDoc.GetElementsByTagName("lastN");
                        lastN = Convert.ToInt32(elemlistCodelastN[0].InnerXml);
                        XmlNodeList elemlistCodeminiStatementList = xmlDoc.GetElementsByTagName("miniStatement");
                        miniStatementList = elemlistCodeminiStatementList[0].InnerXml;
                        foreach (XmlNode node in elemlistCodeminiStatementList)
                        {
                            amount = node["amount"].InnerText;

                            if(node["briefDescriptionEnglish"] != null)
                            {
                                briefDescriptionEnglish = node["briefDescriptionEnglish"].InnerText;
                            }
                            if (node["briefDescriptionArab"] != null)
                            {
                                briefDescriptionArab = node["briefDescriptionArab"].InnerText;
                            }
                            if (node["longDescriptionArab"] != null)
                            {
                                longDescriptionArab = node["longDescriptionArab"].InnerText;
                            }
                            if (node["longDescriptionEnglish"] != null)
                            {
                                longDescriptionEnglish = node["longDescriptionEnglish"].InnerText;
                            }
                            description = node["description"].InnerText;
                            operationNumber = node["operationNumber"].InnerText;
                            postDate = node["postDate"].InnerText;
                            transactionBranch = node["transactionBranch"].InnerText;
                            transactionDate = node["transactionDate"].InnerText;
                            transactionNumber = node["transactionNumber"].InnerText;
                            transactionType = node["transactionType"].InnerText;
                            if(transactionType =="C")
                            {
                                transactionType = "Credit";

                            }
                            else
                            {
                                if(transactionType =="D")
                                {
                                    transactionType= "Debit";
                                }
                            }
                            valueDate = node["valueDate"].InnerText;




                            if (lang == "EN")
                            {
                                datum.Add(new Datum
                                {
                                    amount = amount,
                                    briefDescription = briefDescriptionEnglish,
                                    description = description,
                                    longDescription = longDescriptionEnglish,
                                    operationNumber = operationNumber ,
                                    postDate = postDate,
                                    transactionBranch = transactionBranch,
                                    transactionDate = transactionDate,
                                    transactionNumber= transactionNumber,
                                    transactionType= transactionType,
                                    valueDate= valueDate,
                                    statusCode = statusCode,
                                    statusDesc = statusDesc


                                });
                            }
                            else
                            {
                                if(lang =="AR")
                                {
                                    datum.Add(new Datum
                                    {
                                        amount = amount,
                                        briefDescription = briefDescriptionArab,
                                        description = description,
                                        longDescription= longDescriptionArab,
                                        operationNumber = operationNumber,
                                        postDate = postDate,
                                        transactionBranch = transactionBranch,
                                        transactionDate = transactionDate,
                                        transactionNumber = transactionNumber,
                                        transactionType = transactionType,
                                        valueDate = valueDate,
                                        statusCode = statusCode,
                                        statusDesc = statusDesc
                                    });
                                }
                            }
                     

                        }
                        
                    }
                    else
                    {
                        XmlNodeList elemliststatusCode = xmlDoc.GetElementsByTagName("statusCode");
                        statusCode = elemliststatusCode[0].InnerXml;
                        XmlNodeList elemlistDesc = xmlDoc.GetElementsByTagName("statusDesc");
                        statusDesc = elemlistDesc[0].InnerXml;
                      
                            datum.Add(new Datum
                            {
                                amount = amount,
                          
                                description = description,
                        
                                operationNumber = operationNumber,
                                postDate = postDate,
                                transactionBranch = transactionBranch,
                                transactionDate = transactionDate,
                                transactionNumber = transactionNumber,
                                transactionType = transactionType,
                                valueDate = valueDate,
                                statusCode= statusCode,
                                statusDesc= statusDesc
                            });
                        
              
                    }
                }
            }
            return JsonConvert.SerializeObject((datum));
        }

        public class Datum
        {
    
            public string amount { get; set; }

            public string briefDescription { get; set; }

        

            public string description { get; set; }
            public string longDescription { get; set; }
   
            public string operationNumber { get; set; }

            public string postDate { get; set; }
            public string transactionBranch { get; set; }
            public string transactionDate { get; set; }
            public string transactionNumber { get; set; }
            public string transactionType { get; set; }
            public string valueDate { get; set; }

            public string statusCode { get; set; }

            public string statusDesc { get; set; }






        }
     
        public static HttpWebRequest CreateWebRequestMiniStatment()
        {

            string urlimal = "https://pduatimlappv01.eg.albaraka.local:8743/imal_core_cpws_imal_mig/pathservices/processStatementOfAccount";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlimal);
            webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}
