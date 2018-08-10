using RTLS.Domins.ViewModels;
using RTLS.Domins.ViewModels.OmniRequest;
using RTLS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RTLS.Common;
using Newtonsoft.Json.Linq;
using RTLS.Domains;
using System.Data.Entity;
using RTLS.Domins.ViewModels.Request;
using Newtonsoft.Json;
using System.Text;
using log4net;
using RTLS.Domins.Enums;
using RTLS.Business;

namespace RTLS.API
{
    [RoutePrefix("api/OmniEngine")]
    public class OmniEngineApiController : ApiController,IDisposable
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(OmniEngineApiController));

        public OmniEngineApiController()
        {

        }

        [Route("RegisterDevice")]
        public async Task<HttpResponseMessage> AddDevice(RequestOmniModel objRequestOmniModel)
        {            
            objRequestOmniModel.MacAddress= "7z:c5:37:c0:83:y3";
            //create the RequestModel for secom api
            string result = null;            
            try
            {
                using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
                {
                    //Get the EngageEngine Base Url as per SiteId
                    Site objSiteConfiguration = objRtlsConfigurationRepository.GetAsPerSite(objRequestOmniModel.SiteId);
                    if (objSiteConfiguration.RtlsConfiguration.RtlsEngineType == RtlsEngine.OmniEngine)
                    {
                        OmniEngineBusiness objOmniEngineBusiness = new OmniEngineBusiness();
                        if (await objOmniEngineBusiness.regMacToOmniEngine(objRequestOmniModel))
                        {
                            using (MacAddressRepository objMacAddressRepository = new MacAddressRepository())
                            {
                                objMacAddressRepository.UpdateLocationServiceTypeforMac(objRequestOmniModel);
                            }                            
                        }
                        //string OmniBaseAddressUri = objSiteConfiguration.RtlsConfiguration.OmniBaseAddressUri;
                    }
                    if (objSiteConfiguration.RtlsConfiguration.RtlsEngineType == RtlsEngine.EngageEngine)
                    {
                        EngageEngineBusiness objEngageEngineBusiness = new EngageEngineBusiness();                        
                        if (await objEngageEngineBusiness.regMacToEngageEngine(objRequestOmniModel))
                        {
                            using (MacAddressRepository objMacAddressRepository = new MacAddressRepository())
                            {
                                objMacAddressRepository.UpdateLocationServiceTypeforMac(objRequestOmniModel);
                            }                            
                        }
                        //string EngageBaseAddressUri = objSiteConfiguration.RtlsConfiguration.EngageBaseAddressUri;
                    }
                }                
            }
            catch(Exception ex)
            {
                result = ex.Message;
                log.Error(ex.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
            };

        }
    }
}
