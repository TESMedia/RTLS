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
using RTLS.Repository;

namespace RTLS.Business
{
    public class OmniEngineBusiness : IOmniEngineBusiness
    {
        private readonly OmniDeviceMappingRepository objOmniDeviceMappingRepository = null;
        private readonly MacAddressRepository objMacAddressRepository=null;

        public OmniEngineBusiness()
        {
            objOmniDeviceMappingRepository = new OmniDeviceMappingRepository();
            objMacAddressRepository = new MacAddressRepository();
        }

        public async Task regMacToOmniEngine(RequestOmniModel objRequestOmniModel)
        {
            //ReturnData result = new ReturnData();
            string retResult = null;
            SecomRegisterDevice objSecomRegisterDevice = new SecomRegisterDevice();
            objSecomRegisterDevice.mac = objRequestOmniModel.MacAddress;
            objSecomRegisterDevice.station_info.device.id = objRequestOmniModel.MacAddress;
            //objSecomRegisterDevice.station_info.user.label = objRequestOmniModel.UserName;
            string guest_id = await SaveUserNameToOmniEngine(objRequestOmniModel.UserName);

            string _id = await MapDeviceWithGuestId(objRequestOmniModel, guest_id);

            using (var objSecomClient = new SecomClient())
            {
                var jsonToken = await objSecomClient.GetSecomLoginToken();

                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();

                //Register the Mac
                retResult = await objSecomClient.RegisterDevice(objSecomRegisterDevice, token);

                //If the uniqueId is not retured try with GetDevice to get the uniqueId
                if (string.IsNullOrEmpty(retResult))
                {
                    retResult = await objSecomClient.GetDevice(objRequestOmniModel.MacAddress, token);
                }
                if (!string.IsNullOrEmpty(retResult))
                {
                    await objOmniDeviceMappingRepository.CreateMacUniqueId(objRequestOmniModel.MacAddress, retResult);
                }
            }
        }


        public async Task<List<string>> GetBlacklistDevicesFromOmniEngine()
        {
            //List<string> MacAddressList = null;
            using (var objSecomClient = new SecomClient())
            {
                var jsonToken = await objSecomClient.GetSecomLoginToken();
                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                var MacAddressList = await objSecomClient.getBlacklistDevices(token);
                return MacAddressList;
            }
        }

        public async Task<string> SaveUserNameToOmniEngine(string UserName)
        {
            string returnResult = null;
            using (var objSecomClient = new SecomClient())
            {
                //Get Token Through login
                var jsonToken = await objSecomClient.GetSecomLoginToken();
                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                returnResult = await objSecomClient.AddCpUsers(UserName, token);
                return returnResult;
            }
        }

        public async Task<string> MapDeviceWithGuestId(RequestOmniModel objRequestOmniModel, string guest_id)
        {
            string returnResult = null;
            using (var objSecomClient = new SecomClient())
            {
                //Get Token Through login
                var jsonToken = await objSecomClient.GetSecomLoginToken();
                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                returnResult = await objSecomClient.MapCPUserWithGuestId(objRequestOmniModel, guest_id, token);
                return returnResult;
            }
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

        //DeleteDevices in OmniEngiene
        public async Task<bool> DeleteDevices(RequestOmniModel objRequestOmniModel)
        {
            ReturnData result = new ReturnData();
     

            using (var objSecomClient = new SecomClient())
            {
                //Get Token Through login
                var jsonToken = await objSecomClient.GetSecomLoginToken();

                var token_details = JObject.Parse(jsonToken);
                var token = token_details["jwt"].ToString();
                //Get Unique Id from OmniMapping Table
                var UniqueId = objOmniDeviceMappingRepository.ReregisterGetUniqueId(objRequestOmniModel.MacAddress);
                //call for Delete device 
                var _deleteDevice = await objSecomClient.DeleteDevice(token, UniqueId);
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
