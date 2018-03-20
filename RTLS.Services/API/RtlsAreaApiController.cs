using log4net;
using Microsoft.Owin;
using Newtonsoft.Json;
using RTLS.Business.Repository;
using RTLS.Domains;
using RTLS.Domins;
using RTLS.Domins.ViewModels;
using RTLS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    [Authorize]
    public class RtlsAreaApiController:ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));
        private RtlsAreaApiRepository objRtlsAreaApiRepository;
        private RtlsConfigurationRepository objRtlsConfigurationRepository;
        public RtlsAreaApiController()
        {
            objRtlsAreaApiRepository = new RtlsAreaApiRepository();
            objRtlsConfigurationRepository = new RtlsConfigurationRepository();
        }

        [Route("GetRtlsArea")]
        [HttpPost]
        public HttpResponseMessage GetRtlsArea(RtlsArea model)
        {
            IEnumerable<RtlsArea> objRtlsArea = null;
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            try
            {
                objRtlsArea = objRtlsAreaApiRepository.GetListOfGeoFencedAreasPerSite(model.RtlsConfigurationId);
                jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objRtlsArea, Formatting.None, jsSettings), System.Text.Encoding.UTF8, "application/json")
            };
        }

        [Route("SaveAndUpdateRtlsTrigger")]
        [HttpPost]
        public HttpResponseMessage SaveAndUpdateRtlsTrigger(RtlsAreaViewModel lstRtlsAreas)
        {
            try
            {
                var RtlsConfig=objRtlsConfigurationRepository.GetAsPerSiteId(lstRtlsAreas.SiteId);
                RtlsConfig.ApproachNotification = lstRtlsAreas.ApproachNotification;
                RtlsConfig.AreaNotification = lstRtlsAreas.AreaNotification;
                objRtlsConfigurationRepository.SaveAndUpdateAsPerSite(RtlsConfig);
                List<RtlsArea> lstRtlsArea = new List<RtlsArea>();
                if(lstRtlsAreas.GeoFencedAreas!=null)
                {
                    foreach(var item in lstRtlsAreas.GeoFencedAreas)
                    {
                        RtlsArea objRtlsArea = new RtlsArea();
                        objRtlsArea.GeoFencedAreas = item.GeoFencedAreas;
                        objRtlsArea.RtlsConfigurationId = item.RtlsConfigurationId;                        
                        lstRtlsArea.Add(objRtlsArea);
                    }
                }

                objRtlsAreaApiRepository.SaveAndUpdateAsPerSite(lstRtlsArea.Where(m => m.Id == 0).ToList());
            }
            catch (Exception ex)
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