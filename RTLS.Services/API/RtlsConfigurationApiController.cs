using log4net;
using Newtonsoft.Json;
using RTLS.Domains;
using RTLS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class RtlsConfigurationApiController : ApiController
    {

        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));
        private RtlsConfigurationRepository objRtlsConfigurationRepository;
        public RtlsConfigurationApiController()
        {
            objRtlsConfigurationRepository = new RtlsConfigurationRepository();
        }

        [Route("GetRtlsConfiguration")]
        [HttpPost]
        public HttpResponseMessage GetRtlsConfiguration(Site model)
        {
            Site objSite = null;
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            try
            {
                objSite = objRtlsConfigurationRepository.GetAsPerSite(model.SiteId,model.SiteName);
                jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objSite, Formatting.None, jsSettings), System.Text.Encoding.UTF8, "application/json")
            };
        }


        [Route("SaveAndUpdateRtlsConfiguration")]
        [HttpPost]
        public HttpResponseMessage SaveAndUpdateRtlsConfiguration(RtlsConfiguration model)
        {
            try
            {
                objRtlsConfigurationRepository.SaveAndUpdateAsPerSite(model);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject("Success"), System.Text.Encoding.UTF8, "application/json")
            };
        }


    }
}
