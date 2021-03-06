﻿using RTLS.Domains;
using RTLS.Domins;
using RTLS.Domins.Enums;
using RTLS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RTLS.Business.Repository
{
    public class OmniDeviceMappingRepository
    {
        private ApplicationDbContext db = null;
        private MacAddressRepository _MacAddressRepository = null;

        public OmniDeviceMappingRepository()
        {
            db = new ApplicationDbContext();
            _MacAddressRepository = new MacAddressRepository();
        }
           
        public async Task CreateMacUniqueId(string Mac,string UniqueId)
        {
            try
            {
                if (!db.OmniDeviceMapping.Any(m=>m.Device.MacAddress == Mac))
                {
                    var Device = await _MacAddressRepository.GetDevice(Mac);
                    OmniDeviceMapping objOmniDeviceMapping = new OmniDeviceMapping();
                    objOmniDeviceMapping.DeviceId = Device.DeviceId;
                    objOmniDeviceMapping.UniqueId = UniqueId;
                    db.OmniDeviceMapping.Add(objOmniDeviceMapping);
                    await db.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Return UniqueId for Re-Register
        public string ReregisterGetUniqueId(string Mac)
        {
          
            try
            {
                var DeviceId = GetDeviceId(Mac);

                return  _MacAddressRepository.ReregisterGetUniqueId(DeviceId);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Return deviceId for Macaddress
        public int GetDeviceId(string Mac)
        {
            try
            {
                var Device = _MacAddressRepository.GetDevice(Mac);

               
                return Device.Result.DeviceId;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Return UniqueId for Device
        public string GetUniqueId(string Mac)
        {
            try
            {
                var Device = _MacAddressRepository.GetDevice(Mac);

                string UniqueId = _MacAddressRepository.DeregisterGetUniqueId(Device.Result.DeviceId);
                return UniqueId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
