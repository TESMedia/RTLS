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
            //using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
            //{
            //    //Get the EngageEngine Base Url as per SiteId
            //    string EngineUrl = objRtlsConfigurationRepository.GetAsPerSiteId(objRequestOmniModel.SiteId).EngageBaseAddressUri;
            //}
            //create the RequestModel for secom api

            string result = null;
            try
            {
                SecomRegisterDevice objSecomRegisterDevice = new SecomRegisterDevice();
                objSecomRegisterDevice.mac = objRequestOmniModel.MacAddress;
                objSecomRegisterDevice.station_info.device.id = objRequestOmniModel.MacAddress;

                using (var objSecomClient = new SecomClient())
                {
                    var jsonToken = await objSecomClient.GetSecomLoginToken();

                    var token_details = JObject.Parse(jsonToken);
                    var token = token_details["jwt"].ToString();

                    if (await objSecomClient.RegisterDevice(objSecomRegisterDevice, token))
                    {
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            if (db.Device.Any(m => m.MacAddress == objRequestOmniModel.MacAddress))
                            {
                                var ObjDeviceAssociateSite = db.DeviceAssociateSite.First(m => m.Device.MacAddress == objRequestOmniModel.MacAddress && m.SiteId == objRequestOmniModel.SiteId);
                                if (objRequestOmniModel.NotificationTypeId==10)
                                {
                                    ObjDeviceAssociateSite.IsTrackByAdmin = true;
                                }
                                else if (objRequestOmniModel.NotificationTypeId==20)
                                {
                                    ObjDeviceAssociateSite.IsEntryNotify = true;
                                }
                                ObjDeviceAssociateSite.IsDeviceRegisterInRtls = true;
                                db.Entry(ObjDeviceAssociateSite).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
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
