using System;
using System.Linq;

using RTLS.ViewModel;
using RTLS.Domains;
using RTLS.Domins.Enums;
using RTLS.ReturnModel;
using RTLS.Domins.ViewModels.OmniRequest;
using System.Data.Entity;

namespace RTLS.Repository
{
    public class MacAddressRepository : IDisposable
    {
        private ApplicationDbContext db = null;

        public MacAddressRepository()
        {
            db = new ApplicationDbContext();
        }

        public Device GetDevice(string Mac)
        {
            return db.Device.FirstOrDefault(m => m.MacAddress == Mac);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MacAddress"></param>
        /// <param name="LocationServiceType"></param>
        /// <returns></returns>
        public void UpdateLocationServiceTypeforMac(RequestOmniModel objRequestOmniModel,RtlsEngine EngineType)
        {
            if (db.Device.Any(m => m.MacAddress == objRequestOmniModel.MacAddress))
            {
                var ObjDeviceAssociateSite = db.DeviceAssociateSite.First(m => m.Device.MacAddress == objRequestOmniModel.MacAddress && m.SiteId == objRequestOmniModel.SiteId);
                if (objRequestOmniModel.NotificationTypeId == 10)
                {
                    ObjDeviceAssociateSite.IsTrackByAdmin = true;
                }
                else if (objRequestOmniModel.NotificationTypeId == 20)
                {
                    ObjDeviceAssociateSite.IsEntryNotify = true;
                }
                if(EngineType==RtlsEngine.OmniEngine)
                {
                    ObjDeviceAssociateSite.DeviceRegisteredInEngineType = DeviceRegisteredInEngine.OmniEngine;
                }  
                else if(EngineType == RtlsEngine.EngageEngine)
                {
                    ObjDeviceAssociateSite.DeviceRegisteredInEngineType = DeviceRegisteredInEngine.EngageEngine;
                }              
                db.Entry(ObjDeviceAssociateSite).State = EntityState.Modified;
                db.SaveChanges();
            }
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
                Device objDevice = new Device();
                foreach (var MacAddress in model.MacAddresses)
                {
                    //If MacAddress not exist in the whole system create the new one and associate with Site
                    if (!(db.Device.Any(m => m.MacAddress == MacAddress)))
                    {
                        objDevice.MacAddress = MacAddress;
                        db.Device.Add(objDevice);
                        db.SaveChanges();
                    }
                    //If Device associate Site not exist then Create the new one
                    if (!(db.DeviceAssociateSite.Any(m => m.Device.MacAddress == MacAddress && m.SiteId == model.SiteId)))
                    {
                        objDevice = db.Device.FirstOrDefault(m => m.MacAddress == MacAddress);
                        DeviceAssociateSite objDeviceAssociate = new DeviceAssociateSite();
                        objDeviceAssociate.SiteId = model.SiteId;
                        objDeviceAssociate.DeviceId = objDevice.DeviceId;
                        objDeviceAssociate.CreatedDateTime = DateTime.Now;
                        objDeviceAssociate.IsCreatedByAdmin = true;
                        objDeviceAssociate.IsDeviceRegisterInRtls = true;
                        db.DeviceAssociateSite.Add(objDeviceAssociate);
                    }
                    else
                    {
                        //If the Device Already present then try to update the DeviceAssociateSite 
                        var objDeviceAssociateSite = db.DeviceAssociateSite.FirstOrDefault(m => m.Device.MacAddress == model.Mac && m.SiteId == model.SiteId);
                        objDeviceAssociateSite.IsDeviceRegisterInRtls = true;
                        db.Entry(objDeviceAssociateSite).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
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
                db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool DeRegisterListOfMacs(string[] lstMacAddresses)
        {
            try
            {
                foreach (var item in lstMacAddresses)
                {
                    if (db.Device.Any(m => m.MacAddress == item))
                    {

                        var ObjDevice = db.Device.FirstOrDefault(m => m.MacAddress == item);
                        var objMac = db.DeviceAssociateSite.FirstOrDefault(m => m.DeviceId == ObjDevice.DeviceId);
                        //objMac.status = DeviceStatus.DeRegistered;
                        objMac.status = DeviceStatus.None;
                        //change
                        objMac.DeviceRegisteredInEngineType = DeviceRegisteredInEngine.None;
                        //objMac.IsDeviceRegisterInRtls = false;
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

        public string[] GetMacByStatus(DeviceStatus status)
        {
            string[] macList = (db.DeviceAssociateSite.Where(m => m.status == status).Select(m => m.Device.MacAddress)).ToArray();
            return macList;
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
                                objDevice.DeviceRegisteredInEngineType = DeviceRegisteredInEngine.EngageEngine;
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
                            objDeviceAssociate.DeviceRegisteredInEngineType = DeviceRegisteredInEngine.EngageEngine;
                            //objDeviceAssociate.IsDeviceRegisterInRtls = true;
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
                        //change
                        //objDeviceAssociate.IsDeviceRegisterInRtls = true;
                        objDeviceAssociate.DeviceRegisteredInEngineType = DeviceRegisteredInEngine.EngageEngine;
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