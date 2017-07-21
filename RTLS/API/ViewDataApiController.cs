using log4net;
using Newtonsoft.Json;
using RTLS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RTLS.API
{
    [RoutePrefix("UIData")]
    public class ViewDataApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("ListOfMacAddress")]
        [HttpGet]
        public HttpResponseMessage GetListOfMacAddress()
        {
            List<MacAddress> lstMacAddress = null;
            try
            {
                lstMacAddress = db.MacAddress.ToList();
               
            }
            catch(Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(lstMacAddress), Encoding.UTF8, "application/json")
            };
        }

    }
}
