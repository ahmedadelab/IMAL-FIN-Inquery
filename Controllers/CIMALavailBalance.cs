using Microsoft.AspNetCore.Mvc;

namespace IMAL_FIN_Inquery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CIMALavailBalance : Controller
    {
  

        DLL DLLCOde = new DLL();
        [HttpPost(Name = "SIMALAvailBanlance")]


        public ActionResult<string> OnGet([FromBody] SIMALAvailBanlance x)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var CompanyCode = MyConfig.GetValue<string>("AppSettings:CompanyCode");
            var serviceID = MyConfig.GetValue<string>("AppSettings:serviceIDAvil");
            var serviceDomain = MyConfig.GetValue<string>("AppSettings:serviceDomain");
            var operationName = MyConfig.GetValue<string>("AppSettings:operationNameAvil");
            var businessDomain = MyConfig.GetValue<string>("AppSettings:businessDomain");
            var businessArea = MyConfig.GetValue<string>("AppSettings:businessArea");
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Console.WriteLine(remoteIpAddress.ToString());
            return DLLCOde.ReturnAvailBalance(x.AdditionalRef, x.ChannelName, x.username, x.password, remoteIpAddress, businessArea, businessDomain, operationName, serviceDomain, serviceID, CompanyCode, System.DateTime.Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss"));

        }
    }
}
