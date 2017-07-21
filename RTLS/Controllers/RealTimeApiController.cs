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
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;

namespace RTLS.Controllers
{
    [RoutePrefix("RealTimeLocation")]
    public class RealTimeApiController : ApiController
    {
        private HttpClient httpClient = null;
        private string queryParams = null;
        private string completeFatiAPI = ConfigurationManager.AppSettings["FatiDeviceApi"].ToString();
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(RealTimeApiController));

        public RealTimeApiController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseFatiUriAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]))));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddDevices")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddDevices(RequestLocationDataVM model)
        {
            Notification objNotifications = new Notification();
            try
            {
                queryParams = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "sn", /*model.CompanyName*/ ConfigurationManager.AppSettings["sn"] },
                    { "bn",/* model.SiteName*/ ConfigurationManager.AppSettings["bn"] },
                    {"device_ids",String.Join(",",model.MacAddresses) }
                }).ReadAsStringAsync().Result;

                var result = await httpClient.PostAsync(completeFatiAPI, new StringContent(queryParams, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (result.IsSuccessStatusCode)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    objNotifications = JsonConvert.DeserializeObject<Notification>(resultContent);
                    if (objNotifications.result.returncode == Convert.ToInt32(FatiApiResult.Success))
                    {
                        using (MacAddressRepository objMacRepository = new MacAddressRepository())
                        {
                            objMacRepository.RegisterListOfMacAddresses(model.MacAddresses, model.IscreatedByAdmin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
                objNotifications.result.returncode = -1;
                objNotifications.result.errmsg = ex.InnerException.Message;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetDevices")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDevices(string CompanyName, string SiteName)
        {
            MonitorDevices objMonitorDevice = null;

            //Check the Parameter search or not,if it then add in the QueryParams,else keep as it is
            if (!string.IsNullOrEmpty(CompanyName) && !string.IsNullOrEmpty(SiteName))
            {
                queryParams = new FormUrlEncodedContent(new Dictionary<string, string>()
                 {
                    { "sn", /*model.CompanyName*/ ConfigurationManager.AppSettings["sn"] },
                    { "bn",/* model.SiteName*/ ConfigurationManager.AppSettings["bn"] },
                 }).ReadAsStringAsync().Result;
                completeFatiAPI = completeFatiAPI + "?" + queryParams;
            }

            try
            {
                var result = await httpClient.GetAsync(completeFatiAPI);
                if (result.IsSuccessStatusCode)
                {
                    string resultContent = result.Content.ReadAsAsync<string>().Result;
                    objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(resultContent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objMonitorDevice), Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeRegisterDevices")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeRegisterDevices(RequestLocationDataVM model)
        {
            Notification objNotifications = new Notification();
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(new HttpMethod("DELETE"), "/api/engage/v1/device_monitors/");
                var queryParams = new Dictionary<string, string>()
                {
                    { "sn",ConfigurationManager.AppSettings["sn"] },
                    { "bn",ConfigurationManager.AppSettings["bn"] },
                    {"device_ids",String.Join(",",model.MacAddresses) }
                };

                message.Content = new FormUrlEncodedContent(queryParams);
                try
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(message);
                    if (httpResponseMessage.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        string resultContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        objNotifications = JsonConvert.DeserializeObject<Notification>(resultContent);
                        if (objNotifications.result.returncode == Convert.ToInt32(FatiApiResult.Success))
                        {
                            using (MacAddressRepository objMacRepository = new MacAddressRepository())
                            {
                                objMacRepository.DeRegisterListOfMacs(model.MacAddresses);
                            }
                        }
                    }
                    else
                    {
                        objNotifications.result.returncode = Convert.ToInt32(httpResponseMessage.StatusCode.ToString());
                        objNotifications.result.errmsg = "Some Problem Occured";
                    }
                }
                catch (Exception ex)
                {
                    string errorType = ex.GetType().ToString();
                    string errorMessage = errorType + ": " + ex.Message;
                    throw new Exception(errorMessage, ex.InnerException);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
                objNotifications.result.returncode = -1;
                objNotifications.result.errmsg = ex.InnerException.Message;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }
    }
}
