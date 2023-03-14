using Microsoft.AspNetCore.Mvc;

namespace IMAL_FIN_Inquery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CIMALAccountDetails : Controller
    {

        DLL DLLCOde = new DLL();
        [HttpPost(Name = "SIMALAccountDetails")]
        public ActionResult<string> OnGet([FromBody] SIMALAvailBanlance x)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var CompanyCode = MyConfig.GetValue<string>("AppSettings:CompanyCode");
            var serviceID = MyConfig.GetValue<string>("AppSettings:serviceIDAccD");
            var serviceDomain = MyConfig.GetValue<string>("AppSettings:serviceDomainAccID");
            var operationName = MyConfig.GetValue<string>("AppSettings:operationNameAccID");
            var businessDomain = MyConfig.GetValue<string>("AppSettings:businessDomain");
            var businessArea = MyConfig.GetValue<string>("AppSettings:businessArea");
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Console.WriteLine(remoteIpAddress.ToString());
            return DLLCOde.ReturnGetaccountDetails(x.AdditionalRef, x.ChannelName, x.username, remoteIpAddress, businessArea, businessDomain, operationName, serviceDomain, serviceID, CompanyCode, System.DateTime.Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss"), x.password);
        }
    }
}
