using System;
using System.Linq;

using RTLS.ViewModel;
using RTLS.Domains;
using RTLS.Domins.Enums;
using RTLS.ReturnModel;
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
                        if (!(db.Device.Any(m => m.MacAddress == MacAddress)))
                        {
                            objDevice.MacAddress = MacAddress;
                            db.Device.Add(objDevice);
                            db.SaveChanges();
                        }
                        else
                        {
                            objDevice = db.Device.FirstOrDefault(m => m.MacAddress == MacAddress);
                        }
                    DeviceAssociateSite objDeviceAssociate = new DeviceAssociateSite();
                        objDeviceAssociate.SiteId = model.SiteId;
                        objDeviceAssociate.DeviceId = objDevice.DeviceId;
                        objDeviceAssociate.CreatedDateTime= DateTime.Now;
                        objDeviceAssociate.IsCreatedByAdmin = true;
                        objDeviceAssociate.IsDeviceRegisterInRtls = true;
                        //objDeviceAssociate.RtlsConfigureId = model.RtlsConfigurationId;
                        //objDeviceAssociate.IsCreatedByAdmin = true;
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
                        //db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                        //db.SaveChanges();
                        UpdateStatusToRegister(objMac.DeviceId);
                    }
                    else
                    {
                        Device objMac = new Device();
                        objMac.MacAddress = mac;
                        //objMa = DeviceStatus.Registered;
                        //objMac.RtlsConfigureId = model.RtlsConfigurationId;
                        //objMac.IsCreatedByAdmin = true;
                        db.Device.Add(objMac);
                        db.SaveChanges();
                        UpdateStatusToRegister(objMac.DeviceId);
                    }
                    
                    
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
        public bool UpdateStatusToRegister(int deviceId)
        {
            try
            {
                var ObjDevice = db.DeviceAssociateSite.FirstOrDefault(m => m.DeviceId == deviceId);
                ObjDevice.status = DeviceStatus.Registered;
                ObjDevice.IsDeviceRegisterInRtls = true;
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
                    if (db.Device.Any(m => m.MacAddress == item))
                    { 
                    //var ObjDevice = db.DeviceAssociateSite.FirstOrDefault(m => m.Device.MacAddress == item);
                    //ObjDevice.status = DeviceStatus.DeRegistered;
                    var ObjDevice = db.Device.FirstOrDefault(m => m.MacAddress == item);
                    
                        //db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                        //db.SaveChanges();
                        //UpdateStatusToRegister(objMac.DeviceId);
                        var objMac = db.DeviceAssociateSite.FirstOrDefault(m => m.DeviceId == ObjDevice.DeviceId);
                        //objMac.status = DeviceStatus.DeRegistered;
                        objMac.status = DeviceStatus.None;
                        objMac.IsDeviceRegisterInRtls = false;
                    db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    }
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

        public string CheckDeviceRegisted(MonitorDevices model,int siteid)
        {
            string adminMessage = "";
            try
            {
                foreach (var rec in model.records)
                {
                    //If MacAddress already exist with None status then 
                    if (db.Device.Any(m => m.MacAddress == rec.device_id))
                    {
                        var objMac = db.Device.FirstOrDefault(m => m.MacAddress == rec.device_id);
                        if (db.DeviceAssociateSite.Any(m => m.DeviceId == objMac.DeviceId))
                        {
                            var objDevice = db.DeviceAssociateSite.FirstOrDefault(m => m.DeviceId == objMac.DeviceId);
                            if (objDevice.status != DeviceStatus.Registered)
                            {
                                objDevice.IsDeviceRegisterInRtls = true;
                                objDevice.status = DeviceStatus.Registered;
                                db.Entry(objDevice).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                adminMessage += "[ " + rec.device_id + " ] Change Device Status to Registered." +
                                                Environment.NewLine;
                            }

                        }
                        else
                        {
                            DeviceAssociateSite objDeviceAssociate = new DeviceAssociateSite();
                            objDeviceAssociate.SiteId = siteid;
                            objDeviceAssociate.DeviceId = objMac.DeviceId;
                            objDeviceAssociate.CreatedDateTime = DateTime.Now;
                            objDeviceAssociate.IsDeviceRegisterInRtls = true;
                            db.DeviceAssociateSite.Add(objDeviceAssociate);
                            db.SaveChanges();
                            adminMessage += "[ " + rec.device_id + " ]  Added to Associate Site Mapping." +
                                            Environment.NewLine;
                        }
                    }
                    else
                    {
                        Device objMac = new Device();
                        objMac.MacAddress = rec.device_id;
                        db.Device.Add(objMac);
                        db.SaveChanges();
                        DeviceAssociateSite objDeviceAssociate = new DeviceAssociateSite();
                        objDeviceAssociate.SiteId = siteid;
                        objDeviceAssociate.DeviceId = objMac.DeviceId;
                        objDeviceAssociate.CreatedDateTime = DateTime.Now;
                        objDeviceAssociate.IsDeviceRegisterInRtls = true;
                        objDeviceAssociate.status= DeviceStatus.Registered;
                        db.DeviceAssociateSite.Add(objDeviceAssociate);
                        db.SaveChanges();
                        adminMessage += "[ " + rec.device_id + " ]  Added to Device & Associate Site Mapping." +
                                        Environment.NewLine;
                    }


                }
                if(string.IsNullOrEmpty(adminMessage))
                    adminMessage += "All Device Validated for Registration . Status OK ." +
                                    Environment.NewLine;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return adminMessage;
        }


        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}