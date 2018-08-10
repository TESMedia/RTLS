using Newtonsoft.Json;
using RestSharp;
using RTLS.Domins.Enums;
using RTLS.Domins.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Common
{
    public class EngageEngineClient:IDisposable
    {
        private HttpClient httpClient = null;
        private string queryParams = null;        
        private string _userName = ConfigurationManager.AppSettings["UserName"].ToString();
        private string _password = ConfigurationManager.AppSettings["Password"].ToString();
        private string _completeFatiAPI = ConfigurationManager.AppSettings["FatiDeviceApi"].ToString();        
        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void CommonHeaderInitializeHttpClient(string EngageBaseUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(EngageBaseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", _userName, _password))));
        }

        public async Task<bool> RegisterDevice(EngageRegisterDevice _objEngageRegisterDevice)
        {
            bool _returnData = false;
            Notification objNotifications = new Notification();
            CommonHeaderInitializeHttpClient(_objEngageRegisterDevice.EngageBaseAddressUri);
            try
            {
                queryParams = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "sn", _objEngageRegisterDevice.EngageSiteName },
                    { "bn",_objEngageRegisterDevice.EngageBuildingName },
                    {"device_ids",(_objEngageRegisterDevice.MacAddress) }
                }).ReadAsStringAsync().Result;

                var result = await httpClient.PostAsync(_completeFatiAPI, new StringContent(queryParams, Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (result.IsSuccessStatusCode)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    objNotifications = JsonConvert.DeserializeObject<Notification>(resultContent);
                    if (objNotifications.result.returncode == Convert.ToInt32(FatiApiResult.Success))
                    {
                        _returnData= true;
                    }
                }
            }
            catch (Exception ex)
            {
                _returnData = false;
            }
            return _returnData;  
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
