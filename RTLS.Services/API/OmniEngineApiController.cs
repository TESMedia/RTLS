using RTLS.Domins.ViewModels.OmniRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTLS.API
{
    [RoutePrefix("api/OmniEngine")]
    public class OmniEngineApiController : ApiController
    {
        public OmniEngineApiController()
        {

        }
        
        [Route("RegisterDevice")]
        public HttpResponseMessage AddDevice(RequestOmniModel objRequestOmniModel)
        {
            throw new NotImplementedException();
        }
    }
}
