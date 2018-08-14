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
using RTLS.Business.Repository;

namespace RTLS.Business
{
    public class OmniEngineBusiness : IOmniEngineBusiness
    {
        private OmniDeviceMappingRepository objOmniDeviceMappingRepository = null;

        public OmniEngineBusiness()
        {
            objOmniDeviceMappingRepository = new OmniDeviceMappingRepository();
        }

        public async Task<ReturnData> regMacToOmniEngine(RequestOmniModel objRequestOmniModel)
        {
            ReturnData result = new ReturnData();
            SecomRegisterDevice objSecomRegisterDevice = new SecomRegisterDevice();
            objSecomRegisterDevice.mac = objRequestOmniModel.MacAddress;
            objSecomRegisterDevice.station_info.device.id = objRequestOmniModel.MacAddress;

            using (var objSecomClient = new SecomClient())
            {
                var jsonToken = await objSecomClient.GetSecomLoginToken();

                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                var registerResult = await objSecomClient.RegisterDevice(objSecomRegisterDevice, token);
                result.Status = registerResult.Status;
                result.UniqueId = registerResult.UniqueId;  
                if(registerResult.Status==true)
                {
                    objOmniDeviceMappingRepository.CreateMacUniqueId(objRequestOmniModel.MacAddress, registerResult.UniqueId);
                }                             
            }
            return result;
        }
        //Reregister in OmniEngiene
        public async Task<bool> ReRegister(RequestOmniModel objRequestOmniModel)
        {
            ReturnData result = new ReturnData();
            SecomRegisterDevice objSecomRegisterDevice = new SecomRegisterDevice();
            objSecomRegisterDevice.mac = objRequestOmniModel.MacAddress;
            objSecomRegisterDevice.station_info.device.id = objRequestOmniModel.MacAddress;

            using (var objSecomClient = new SecomClient())
            {
                //Get Token Through login
                var jsonToken = await objSecomClient.GetSecomLoginToken();

                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                //Get Unique Id from OmniMapping Table
                var UniqueId = objOmniDeviceMappingRepository.ReregisterGetUniqueId(objRequestOmniModel.MacAddress);
                //call for deregister mac 
                var _reriegister = await objSecomClient.ReRegisterDevice(objSecomRegisterDevice, token, UniqueId);
                return true;
            }

        }
        //Deregister Mac FromOmniEngine
        public async Task<bool> DeregisterMacFromOmniEngine(RequestOmniModel objRequestOmniModel)
        {
            ReturnData result = new ReturnData();
            SecomRegisterDevice objSecomRegisterDevice = new SecomRegisterDevice();
            objSecomRegisterDevice.mac = objRequestOmniModel.MacAddress;
            objSecomRegisterDevice.station_info.device.id = objRequestOmniModel.MacAddress;

            using (var objSecomClient = new SecomClient())
            {
                //Get Token Through login
                var jsonToken = await objSecomClient.GetSecomLoginToken();

                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                //Get Unique Id from OmniMapping Table
                var UniqueId=objOmniDeviceMappingRepository.GetUniqueId(objRequestOmniModel.MacAddress); 
               //call for deregister mac 
               var _deriegister=await objSecomClient.DeRegisterDevice(objSecomRegisterDevice, token,UniqueId);
                return true;
            }
            
        }
    }
}
