using RTLS.Domains;
using RTLS.Domins;
using RTLS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
           
        public void CreateMacUniqueId(string Mac,string UniqueId)
        {
            try
            {
                var Device = _MacAddressRepository.GetDevice(Mac);
                OmniDeviceMapping objOmniDeviceMapping = new OmniDeviceMapping();
                //objOmniDeviceMapping.Device = Device;
                objOmniDeviceMapping.DeviceId = Device.DeviceId;
                objOmniDeviceMapping.UniqueId = UniqueId;
                db.OmniDeviceMapping.Add(objOmniDeviceMapping);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        } 
    }
}
