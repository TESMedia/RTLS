using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RTLS.Models;
using System.Configuration;
using RTLS.ViewModel;
using RTLS.Repository;
using Newtonsoft.Json;
using RTLS.ReturnModel;
using RTLS.ServiceReturn;
using RTLS.Enum;
using log4net;
using System.Text;

namespace RTLS.Controllers
{
    [RoutePrefix("RealTimeLocation")]
    public class RealTimeApiController : ApiController
    {
        private ApplicationDbContext db = null;
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AdminController));
        public RealTimeApiController()
        {
            db = new ApplicationDbContext();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       [Route("AddDevices")]
       [HttpPost]
       public HttpResponseMessage AddDevices(RequestLocationDataVM model)
        {
            Result objResult = null;
            try
            {
                EngageLocations objEngageApi = new EngageLocations();
                var retResult=objEngageApi.AddDeviceLocationRestClient(model.CompanyName, model.SiteName, model.MacAddress);
                objResult=JsonConvert.DeserializeObject<Result>(retResult);
            }
            catch(Exception ex)
            {

            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objResult), Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetDevicesFilter")]
        public HttpResponseMessage GetDevices(RequestLocationDataVM model)
        {
            MonitorDevices objMonitorDevice = null;
            try
            {
                EngageLocations objApiCall = new EngageLocations();
                string strResult = objApiCall.GetAllDeviceDetails();
                objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(strResult);
            }
            catch (Exception ex)
            {

            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objMonitorDevice), Encoding.UTF8, "application/json")
            };
        }

        [Route("DeleteDevices")]
        public HttpResponseMessage DeleteDevice(RequestLocationDataVM model)
        {
            Result objResult = null;
            try
            {
                EngageLocations objApiCall = new EngageLocations();
                string strResult = objApiCall.DeleteDeviceLocation(model.CompanyName,model.SiteName,model.MacAddress);
                objResult = JsonConvert.DeserializeObject<Result>(strResult);
            }
            catch (Exception ex)
            {

            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objResult), Encoding.UTF8, "application/json")
            };
        }

    }
}
