using RTLS.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTLS.Domins.ViewModels.OmniRequest;
using RTLS.Domins.ViewModels;
using RTLS.Common;
using RTLS.Domains;
using Newtonsoft.Json.Linq;

namespace RTLS.Business
{
    public class OmniEngineBusiness : IOmniEngineBusiness
    {
        public async Task<bool> regMacToOmniEngine(RequestOmniModel objRequestOmniModel)
        {
            bool result = false;
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
                    result= true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
