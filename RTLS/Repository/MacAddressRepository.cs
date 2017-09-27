using System;
using System.Linq;
using RTLS.Models;
using RTLS.Enum;
using RTLS.ViewModel;

namespace RTLS.Repository
{
    public class MacAddressRepository : IDisposable
    {
        private ApplicationDbContext db = null;

        public MacAddressRepository()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="Mac"></param>
        /// <returns></returns>
        public bool SaveMacAddress(RequestLocationDataVM model)
        {
            try
            {
                foreach (var Item in model.MacAddresses)
                {
                    if (!(db.Device.Any(m => m.MacAddress == Item && m.RtlsConfigureId==model.RtlsConfigurationId)))
                    {
                        Device objMac = new Device();
                        objMac.MacAddress = Item;
                        objMac.status = DeviceStatus.None;
                        objMac.CreatedDateTime = DateTime.Now;
                        objMac.IsCreatedByAdmin = true;
                        objMac.RtlsConfigureId = model.RtlsConfigurationId;
                        db.Device.Add(objMac);
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstMac"></param>
        /// <param name="SiteName"></param>
        /// <returns></returns>
        public bool RegisterListOfMacAddresses(RequestLocationDataVM model)
        {
            try
            {
                foreach (var mac in model.MacAddresses)
                {
                    //If MacAddress already exist with None status then 
                    if (db.Device.Any(m => m.MacAddress == mac && m.status == DeviceStatus.None))
                    {
                        var objMac = db.Device.FirstOrDefault(m => m.MacAddress == mac);
                        objMac.status = DeviceStatus.Registered;
                        db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Device objMac = new Device();
                        objMac.MacAddress = mac;
                        objMac.status = DeviceStatus.Registered;
                        objMac.RtlsConfigureId = model.RtlsConfigurationId;
                        objMac.IsCreatedByAdmin = true;
                        db.Device.Add(objMac);

                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MacId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool UpdateStatusToRegister(int MacId)
        {
            try
            {
                var ObjDevice = db.Device.FirstOrDefault(m => m.Id == MacId);
                ObjDevice.status = DeviceStatus.Registered;
                db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool DeRegisterListOfMacs(String[] MacAddresses)
        {
            try
            {
                foreach (var item in MacAddresses)
                {
                    var ObjDevice = db.Device.FirstOrDefault(m => m.MacAddress == item);
                    ObjDevice.status = DeviceStatus.DeRegistered;
                    db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool CheckListExistOrNot(string[] lstMac,int RtlsConfigureId)
        {
            var difference = lstMac.Except(db.Device.Where(m=>m.RtlsConfigureId == RtlsConfigureId).Select(m => m.MacAddress));
            if (difference.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] GetMacAdressFromId(int MacId)
        {
            return new[] { db.Device.FirstOrDefault(m => m.Id == MacId).MacAddress };
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}