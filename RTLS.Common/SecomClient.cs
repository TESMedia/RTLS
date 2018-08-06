using Newtonsoft.Json;
using RTLS.Domins.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using RestSharp;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading;

namespace RTLS.Common
{
    public class SecomClient:IDisposable
    {
        
        private string _userName = ConfigurationManager.AppSettings["SecomLoginUname"].ToString();
        private string _password = ConfigurationManager.AppSettings["SecomLoginPassword"].ToString();
        private string _uri = ConfigurationManager.AppSettings["SecomAPI"].ToString();
        private bool disposed;

        public async Task<string> GetSecomLoginToken()
            
        {
            string _returnData=null;
            var _secomLoginData = new Dictionary<string, string>()
                      {
                        { "username",_userName },
                        { "password",_password }

                      };

            //// Serialize our concrete class into a JSON String
            var _loginData = JsonConvert.SerializeObject(_secomLoginData);

            //Rest CLient Call
            var restClient = new RestClient();
            restClient.BaseUrl = new Uri(_uri);
            var restRequest = new RestRequest("POST");
            restRequest.Resource = "api/v1/accounts/login";
            restRequest.AddHeader("Content-yType", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", _loginData, ParameterType.RequestBody);

            var response = await (Task.Run(() => restClient.Post(restRequest)));
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _returnData = response.Content.ToString();
            }
            return _returnData;                
        }



        
        public async Task<string> RegisterDevice(SecomRegisterDevice _objSecomRegisterDevice,string token)

        {
            string _returnData = null;
            
            //// Serialize our concrete class into a JSON String
            var _registerData = JsonConvert.SerializeObject(_objSecomRegisterDevice);
             
            //Rest CLient Call
            var restClient = new RestClient();
            restClient.BaseUrl = new Uri(_uri);
            var restRequest = new RestRequest("POST");
            restRequest.Resource = "api/v1/venues/devices";
            restRequest.AddHeader("Content-yType", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Authorization","Bearer"+" "+token);
            restRequest.AddParameter("application/json", _registerData, ParameterType.RequestBody);


            var response = await (Task.Run(() => restClient.Post(restRequest)));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _returnData = response.Content.ToString();
            }
            else
            {
                _returnData = response.Content.ToString();
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
