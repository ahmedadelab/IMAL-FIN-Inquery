using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;

namespace IMAL_FIN_Inquery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CIMALProcessStatment : Controller
    {
     
        DLL DLLCOde = new DLL();
        [HttpPost(Name = "SIMALProcessStatment")]
        public ActionResult<string> OnGet([FromBody] SIMALProcessStatment x)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var CompanyCode = MyConfig.GetValue<string>("AppSettings:CompanyCode");
            var serviceID = MyConfig.GetValue<string>("AppSettings:serviceID");
            var serviceDomain = MyConfig.GetValue<string>("AppSettings:serviceDomain");
            var operationName = MyConfig.GetValue<string>("AppSettings:operationName");
            var businessDomain = MyConfig.GetValue<string>("AppSettings:businessDomain");
            var businessArea = MyConfig.GetValue<string>("AppSettings:businessArea");
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            Console.WriteLine(remoteIpAddress.ToString());
            return  (DLLCOde.returnMiniStatment(x.AdditionalRef,x.username,x.password,CompanyCode,serviceID,serviceDomain,operationName,businessDomain,businessArea,x.LastN,System.DateTime.Now.ToString("yyyy-MM-dd"+"T"+"HH:mm:ss"),x.Lang));
           
        }
    }
}
