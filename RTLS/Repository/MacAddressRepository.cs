using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RTLS.Models;
using RTLS.Enum;

namespace RTLS.Repository
{
    public class MacAddressRepository:IDisposable
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
        public bool SaveMacAddress(string [] lstMac,bool IsCreateFromAdmin)
        {
            try
            {
                foreach(var Item in lstMac)
                {
                    if (!(db.MacAddress.Any(m => m.Mac == Item)))
                    {
                        MacAddress objMac = new MacAddress();
                        objMac.Mac = Item;
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.None);
                        objMac.IsCreatedByAdmin = IsCreateFromAdmin;
                        db.MacAddress.Add(objMac);
                        db.SaveChanges();  
                    }
                }
                return true;
            }
            catch(Exception ex)
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
        public bool RegisterListOfMacAddresses(string[] lstMac,bool IsCreatedByAdmin)
        {
            try
            {
                foreach (var mac in lstMac)
                {
                    MacAddress objMac = new MacAddress();
                    objMac.Mac = mac;
                    objMac.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                    objMac.IsCreatedByAdmin = IsCreatedByAdmin;
                    db.MacAddress.Add(objMac);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
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
                var ObjDevice = db.MacAddress.FirstOrDefault(m => m.Id == MacId);
                ObjDevice.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool DeRegisterListOfMacs(String [] MacAddresses)
        {
            try
            {
                foreach(var item in MacAddresses)
                {
                    var ObjDevice = db.MacAddress.FirstOrDefault(m => m.Mac == item);
                    ObjDevice.Intstatus = Convert.ToInt32(DeviceStatus.DeRegistered);
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


        public bool CheckMacAddressExitOrNot(int MacId,int IntStatus)
        {
            if (db.MacAddress.FirstOrDefault(m => m.Id == MacId).Intstatus != IntStatus)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckListExistOrNot(string [] lstMac,int IntStatus)
        {
            var difference = db.MacAddress.Select(m=>m.Mac).Except(lstMac);
            if(difference.Count()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string [] GetMacAdressFromId(int MacId)
        {
            return new[] { db.MacAddress.FirstOrDefault(m => m.Id == MacId).Mac };
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}