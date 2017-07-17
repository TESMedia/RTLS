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
            Result objResult = new Result();
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
                    objResult = JsonConvert.DeserializeObject<Result>(resultContent);
                    if (objResult.returncode==Convert.ToInt32(FatiApiResult.Success))
                    {
                        using (MacAddressRepository objMacRepository = new MacAddressRepository())
                        {
                            objMacRepository.RegisterListOfMacAddresses(model.MacAddresses,model.IscreatedByAdmin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
                objResult.returncode = -1;
                objResult.errmsg = ex.InnerException.Message;
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
        [Route("DeleteDevices")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteDevice(RequestLocationDataVM model)
        {
            Result objResult = new Result();
            try
            { 
                HttpRequestMessage message = new HttpRequestMessage(new HttpMethod("DELETE"), "/api/engage/v1/device_monitors/");
                message.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
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
                        objResult = JsonConvert.DeserializeObject<Result>(resultContent);
                        if (objResult.returncode == Convert.ToInt32(FatiApiResult.Success))
                        {
                            using (MacAddressRepository objMacRepository = new MacAddressRepository())
                            {
                                objMacRepository.DeRegisterListOfMacs(model.MacAddresses);
                            }
                        }
                    }
                    else
                    {
                        objResult.returncode=Convert.ToInt32(httpResponseMessage.StatusCode.ToString());
                        objResult.errmsg ="Some Problem Occured";
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
                objResult.returncode = -1;
                objResult.errmsg = ex.InnerException.Message;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objResult), Encoding.UTF8, "application/json")
            };
        }
    }
}
