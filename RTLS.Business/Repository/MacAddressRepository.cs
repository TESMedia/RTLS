using System;
using System.Linq;

using RTLS.ViewModel;
using RTLS.Domains;
using RTLS.Domins.Enums;

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
                foreach (var MacAddress in model.MacAddresses)
                {
                    if (!(db.DeviceAssociateSite.Any(m => m.Device.MacAddress == MacAddress && m.SiteId==model.SiteId)))
                    {
                        Device objDevice = new Device();
                        objDevice.MacAddress = MacAddress;
                        //objMac. = DeviceStatus.None;
                        //objMac.CreatedDateTime = DateTime.Now;
                        //objMac.IsCreatedByAdmin = true;
                        //objMac.RtlsConfigureId = model.RtlsConfigurationId;
                        db.Device.Add(objDevice);
                        db.SaveChanges();

                        DeviceAssociateSite objDeviceAssociate = new DeviceAssociateSite();
                        objDeviceAssociate.SiteId = model.SiteId;
                        objDeviceAssociate.DeviceId = objDevice.DeviceId;
                        db.DeviceAssociateSite.Add(objDeviceAssociate);
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
                    if (db.Device.Any(m => m.MacAddress == mac))
                    {
                        var objMac = db.Device.FirstOrDefault(m => m.MacAddress == mac);
                        db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Device objMac = new Device();
                        objMac.MacAddress = mac;
                        //objMac.status = DeviceStatus.Registered;
                        //objMac.RtlsConfigureId = model.RtlsConfigurationId;
                        //objMac.IsCreatedByAdmin = true;
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
                var ObjDevice = db.DeviceAssociateSite.FirstOrDefault(m => m.Id == MacId);
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

        public bool DeRegisterListOfMacs(string [] lstMacAddresses)
        {
            try
            {
                foreach (var item in lstMacAddresses)
                {
                    var ObjDevice = db.DeviceAssociateSite.FirstOrDefault(m => m.Device.MacAddress == item);
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

        public bool CheckListExistOrNot(string[] lstMac,int SiteId)
        {
            var difference = lstMac.Except(db.DeviceAssociateSite.Where(m=>m.SiteId == SiteId).Select(m => m.Device.MacAddress));
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
            return new[] { db.Device.FirstOrDefault(m => m.DeviceId == MacId).MacAddress };
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}