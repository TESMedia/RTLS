using log4net;
using Newtonsoft.Json;
using RTLS.Domains;
using RTLS.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("UIData")]
    public class ViewDataApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("ListOfMacAddress")]
        [HttpPost]
        public HttpResponseMessage GetListOfMacAddress(RequestLocationDataVM model)
        {
            IEnumerable Maclist=null;
            try
            {
                if(db.RtlsConfiguration.Any(m=>m.SiteId== model.SiteId))
                {
                    Maclist = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId).Select(m=>new {Id=m.Id, Mac=m.Device.MacAddress, StrStatus=m.status.ToString(), IsDisplay=m.IsDeviceRegisterInRtls }).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(Maclist), Encoding.UTF8, "application/json")
            };
        }
    }
}
