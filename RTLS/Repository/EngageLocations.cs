using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using System.IO;
using System.Collections.Specialized;
using RestSharp.Authenticators;
using System.Text;
using System.Configuration;
using ZeroMQ;
using Newtonsoft.Json;
using RTLS.Models;
using System.Net.Sockets;

namespace RTLS.Repository
{
    public class EngageLocations
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAllDeviceDetails()
        {
            string result = "";
            RestClient client = new RestClient(ConfigurationManager.AppSettings["BaseFatiUriAddress"]);
            client.Authenticator = new HttpBasicAuthenticator("airloc8", "fXwmhe5uZFxj5hYK");
            var request = new RestRequest("/api/engage/v1/device_monitors/", Method.GET);
            request.AddHeader("Authorization", "Basic YWlybG9jODpmWHdtaGU1dVpGeGo1aFlL");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            var response = client.Execute(request);
            result = response.Content;
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="bn"></param>
        /// <returns></returns>
        public string GetAllDeviceDetailsAsPerSnBn(string sn, string bn)
        {
            RestClient client = new RestClient(ConfigurationManager.AppSettings["BaseFatiUriAddress"]);
            client.Authenticator = new HttpBasicAuthenticator("airloc8", "fXwmhe5uZFxj5hYK");

            var queryParams = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "sn", sn },
                { "bn", bn },
            }).ReadAsStringAsync().Result.Replace("+", "%20").ToString();

            var request = new RestRequest("/api/engage/v1/device_monitors/", Method.GET);
            request.AddHeader("Authorization", "Basic YWlybG9jODpmWHdtaGU1dVpGeGo1aFlL");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("sn", sn, "application/x-www-form-urlencoded", ParameterType.QueryString);
            request.AddParameter("bn", bn, "application/x-www-form-urlencoded", ParameterType.QueryString);
            var response = client.Execute(request).Content;
            return response; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="bn"></param>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public  async Task AddDeviceLocation(string sn,string bn,string DeviceId)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseFatiUriAddress"]);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
 

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sn", "FEUTUI001 - Thomson Cruises");
            parameters.Add("bn", "Discovery 1");
            parameters.Add("device_ids", "fc:92:3b:3d:35:80");

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                if (!string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                {
                    if (sb.Length > 0) sb.Append('&');
                    sb.Append(HttpUtility.UrlEncode(kvp.Key));
                    sb.Append('=');
                    sb.Append(HttpUtility.UrlEncode(kvp.Value));
                }
            }
            string sr = sb.ToString().Replace("+", "%20").Replace("%3a", ":");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "airloc8", "fXwmhe5uZFxj5hYK"))));
            var result = await httpClient.PostAsync("/api/engage/v1/device_monitors/", new StringContent(sr, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="bn"></param>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public string AddDeviceLocationRestClient(string sn, string bn, string DeviceId)
        {
            IRestResponse response = null;
            try
            {
                RestClient client = new RestClient(ConfigurationManager.AppSettings["BaseFatiUriAddress"]);
                client.Authenticator = new HttpBasicAuthenticator("airloc8", "fXwmhe5uZFxj5hYK");
                var request = new RestRequest("/api/engage/v1/device_monitors/", Method.POST);
                request.AddHeader("Authorization", "Basic YWlybG9jODpmWHdtaGU1dVpGeGo1aFlL");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("sn", sn);
                parameters.Add("bn", bn);
                parameters.Add("device_ids", DeviceId);

                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    if (!string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                    {
                        if (sb.Length > 0) sb.Append('&');
                        sb.Append(HttpUtility.UrlEncode(kvp.Key));
                        sb.Append('=');
                        sb.Append(HttpUtility.UrlEncode(kvp.Value));
                    }
                }

                string sr = sb.ToString().Replace("+", "%20").Replace("%3a", ":");
                request.AddParameter("application/x-www-form-urlencoded", sr, ParameterType.RequestBody);
                request.AddBody(sr);
                response = client.Execute(request);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return response.Content;
        }


        public string DeleteDeviceLocation(string sn, string bn, string DeviceId)
        {
            RestClient client = new RestClient(ConfigurationManager.AppSettings["BaseFatiUriAddress"]);
            client.Authenticator = new HttpBasicAuthenticator("airloc8", "fXwmhe5uZFxj5hYK");
            var request = new RestRequest("/api/engage/v1/device_monitors/", Method.DELETE);
            request.AddHeader("Authorization", "Basic YWlybG9jODpmWHdtaGU1dVpGeGo1aFlL");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sn", sn);
            parameters.Add("bn", bn);
            parameters.Add("device_ids", DeviceId);

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                if (!string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                {
                    if (sb.Length > 0) sb.Append('&');
                    sb.Append(HttpUtility.UrlEncode(kvp.Key));
                    sb.Append('=');
                    sb.Append(HttpUtility.UrlEncode(kvp.Value));
                }
            }
            string sr = sb.ToString().Replace("+", "%20").Replace("%3a", ":");

            request.AddParameter("application/x-www-form-urlencoded", sr, ParameterType.RequestBody);
            request.AddBody(sr);

            var response = client.Execute(request);
            return response.Content;
        }


        public void GetAllNotification()
        {
            try
            {
                List<LocationData> lstLocationData = new List<LocationData>();
                List<Area> lstArea = new List<Area>();
                string topic = "device";
                string url = "tcp://172.19.255.38:5560";
                using (var context = new ZContext())
                using (var subscriber = new ZSocket(context, ZSocketType.SUB))
                {
                    subscriber.Connect("tcp://172.19.255.38:5560");
                    Console.WriteLine("Subscriber started for Topic with URL : {0} {1}", topic, url);
                    subscriber.Subscribe(topic);
                    int subscribed = 0;

                    string str = "{\"device_notification\":{\"records\": [{\"mac\":\"fc: 25:3f:5d:10:54\",\"sequence\":44,\"sn\":\"FEUTUI001 - Thomson Cruises\",\"bn\":\"Discovery 1\",\"fn\":\"Deck0\",\"x\":173,\"Y\":18,\"z\":0,\"last_seen_ts\":1497861507,\"action\":\"not_moved\",\"fix_result\":\"SUCCESS\",\"an\":[\"PQU0000D0QUSBF\",\"PQU0000D0QUSBF\"]}]}}";


                    while (true)
                    {

                        using (ZMessage message = subscriber.ReceiveMessage())
                        {
                            subscribed++;

                            // Read envelope with address
                            string address = message[0].ReadString();

                            // Read message contents
                            string contents = message[1].ReadString();

                            LocationData objLocationData = JsonConvert.DeserializeObject<ListOfArea>(str).device_notification.records.FirstOrDefault();
                            foreach (var item in objLocationData.an)
                            {
                                objLocationData.AreaName = item;
                                lstLocationData.Add(objLocationData);
                            }
                            db.LocationData.AddRange(lstLocationData);
                            // System.IO.File.WriteAllText(@"C:\Users\Dell\Documents\Visual Studio 2015\Projects\ZeroMqApp\ZeroMqApp\Files\WriteLines.txt", contents);
                            Console.WriteLine("{0}. [{1}] {2}", subscribed, address, contents);
                            db.SaveChanges();
                        }  
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
