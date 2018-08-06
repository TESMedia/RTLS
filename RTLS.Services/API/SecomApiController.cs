using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RTLS.Common;
using RTLS.Domains;
using RTLS.Domins.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class SecomApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("secom/v1/login")]
        public async Task<HttpResponseMessage> SecomLogin()
        {
            try
            {
                using (SecomClient objsecomClient = new SecomClient())
                {


                    var _secomData = await objsecomClient.GetSecomLoginToken();

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(_secomData, Encoding.UTF8, "application/json")
                    };
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        [HttpPost]
        [Route("secom/v1/venues/RegisterDevice")]
        public async Task<HttpResponseMessage> RegisterDevice(SecomRegisterDevice _objSecomRegisterDevice)
        {
            string _registerSuccessData = null;

            using (SecomClient objsecomClient = new SecomClient())
            {
                //Call getToken method
                var _secomData = await objsecomClient.GetSecomLoginToken();


                var token_details = JObject.Parse(_secomData);
                var token = token_details["jwt"].ToString();


                //Calling register device
                _registerSuccessData = await objsecomClient.RegisterDevice(_objSecomRegisterDevice, token);

            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(_registerSuccessData, Encoding.UTF8, "application/json")
            };

        }



    }
}
