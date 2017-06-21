using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RTLS.Models;
using RTLS.ReturnModel;
using RTLS.Services;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using RTLS.ServiceReturn;
using RTLS.ViewNotification;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using System.Reflection;
using RTLS.Repository;
using System.Threading.Tasks;
using RTLS.Enum;
using Newtonsoft.Json.Linq;

namespace RTLS.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        private ApplicationDbContext db = new ApplicationDbContext();

        private TestService objtestService = new TestService();
        private JavaScriptSerializer objSerialization = new JavaScriptSerializer();


        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AdminController));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            List<MacAddress> lstMacAddress = db.MacAddress.ToList();
            return View(lstMacAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="chkMacDevices"></param>
        /// <param name="txtMacDevices"></param>
        /// <returns></returns>
        [HttpPost]
        public  ActionResult Index(string Command,int [] chkMacDevices,string [] txtMacDevices,int ddlSelectCompany)
        {
            TempData["MacAddresses"] = chkMacDevices;
            //log.Error("error");
            //log.Fatal("fatal");
            //log.Warn("warning");
            //log.Info("Get the MacAddress");

            if (Command == "Save")
            {
                SaveMacAddressInDb(txtMacDevices, ddlSelectCompany);
            }
            else if(Command=="Register")
            {
                AddDeviceIds(chkMacDevices);
            }
            else if (Command == "Get Latest Position")
            {
                return RedirectToActionPermanent("GetLatestPosition",new { CompanyId= ddlSelectCompany });
            }
            else if (Command == "ViewMacAddress")
            {

            }
            else if (Command == "De-Register")
            {
                DeRegisterMacAddress(chkMacDevices);
            }
            else if (Command == "Delete Mac Address")
            {
                DeleteMacAddress(chkMacDevices);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Authenticate(LoginViewModel model)
        {
            try
            {
                this.log.Debug("Enter into the Authentication Action Method");
                objtestService.Authenticate(model);
                Success(string.Format("{0},  successfully Authenticated.", model.UserName), true);
            }
            catch(Exception ex)
            {
                this.log.Error("Exception occur"+ex.Message);
                Danger("Looks like something went wrong. Please check your form.");
            }
            return RedirectToAction("Index");
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        public void SaveMacAddressInDb(string [] txtMacDevices,int CompanyId)
        {
  
            try
            {
                this.log.Debug("Enter into the SaveMacAddressInDb Action Method");
                foreach (var item in txtMacDevices)
                {
                    if(!(db.MacAddress.Any(m=>m.Mac==item)))
                    {

                        MacAddress objMac = new MacAddress();
                        objMac.CompnayId = CompanyId;
                     
                        objMac.Mac = item;
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.None);
                        db.MacAddress.Add(objMac);
                        db.SaveChanges();
                        Success(string.Format("{0}{1} Successfully Saved into database.", item, "success"), true);
                    }
                    else
                    {
                        Warning(string.Format("{0}{1} MacAddress exist",item,"Warning"),true);
                    }
                   
                }
                
            }
            catch(Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
                Danger(string.Format("{0} Failed to store into database.", "Warning"), true);
            }
        }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="chkMacDevices"></param>
         /// <returns></returns>
        public void AddDeviceIds(int [] macDevices)
        {
            try
            {
                this.log.Debug("Enter into the AddDeviceIds Action Method");
                string strResult = null;
                foreach (var item in macDevices)
                {
                    var ObjDevice = db.MacAddress.FirstOrDefault(m => m.Id == item);
                    string mac = ObjDevice.Mac;
                    if (db.MacAddress.FirstOrDefault(m => m.Id == item).Intstatus != Convert.ToInt32(DeviceStatus.Registered))
                    {
                        var objCompany = db.Company.FirstOrDefault(m => m.CompnayId == ObjDevice.CompnayId);
                        var objSite = db.Site.FirstOrDefault(m => m.SiteId == objCompany.SiteId);

                        //Call the API to register one one device 
                        EngageLocations objApiCall = new EngageLocations();
                        strResult = objApiCall.AddDeviceLocationRestClient(objCompany.CompanyName, objSite.SiteName, ObjDevice.Mac);
                        ReturnNew objResult = JsonConvert.DeserializeObject<ReturnNew>(strResult);

                        

                        if (objResult.result.returncode == 0)
                        {
                            //Change the Status to Registered after getting success
                            ObjDevice.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                            db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            //Notify the Success after the registeration successfully
                            Success(string.Format("{0} Successfully Registered into server.", mac), true);
                        }
                        else
                        {
                            //Notify the failure after registration failed
                            Danger(string.Format("{0}", objResult.result.errmsg), true);
                        }

                    }
                    else
                    {
                        Warning(string.Format("{0}",mac+"MacAddress already Registered"), true);
                    }
                }
               
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
                Danger(string.Format("{0}" + ex.Message), true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionResult GetDeviceIds(int[] chkMacDevices)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteMacAddress(int [] chkMacDevices)
        {
            try
            {
                this.log.Debug("Enter into the DeleteMacAddress Action Method");
                Result objResult = null;
                string strResult = objtestService.DeleteDevicesMonitor(chkMacDevices);
                objResult = JsonConvert.DeserializeObject<Result>(strResult);

                if (Convert.ToUInt32(objResult.returncode) == ServiceResult.Success)
                {
                    foreach (var item in chkMacDevices)
                    {
                        var deviceObject = db.MacAddress.FirstOrDefault(m => m.Id == item);
                        string mac = deviceObject.Mac;
                        if (deviceObject.Intstatus != Convert.ToInt32(DeviceStatus.Registered))
                        {
                            db.MacAddress.Remove(deviceObject);
                            db.SaveChanges();
                            Success(string.Format("{0} Successfully Registered into server.", mac), true);
                        }
                        else
                        {
                            Warning(string.Format("{0} is a Registered User So shouldn't Delete.", mac), true);
                        }
                    }
                }
                else
                {
                    Warning(string.Format("{0}" + objResult.errmsg), true);
                }
            }
            catch(Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
                Danger(string.Format("{0}" + ex.Message), true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void DeRegisterMacAddress(int[] chkMacDevices)
        {
            try
            {
                this.log.Debug("Enter into the DeRegisterMacAddress Action Method");

                foreach (var item in chkMacDevices)
                {
                    var deviceObject = db.MacAddress.FirstOrDefault(m => m.Id == item);
                    string mac = deviceObject.Mac;
                    if (db.MacAddress.FirstOrDefault(m => m.Id == item).Intstatus != Convert.ToInt32(DeviceStatus.DeRegistered))
                    {
                        EngageLocations objEngage = new EngageLocations();
                        objEngage.DeleteDeviceLocation(deviceObject.Company.CompanyName, deviceObject.Company.Site.SiteName, mac);
                        deviceObject.Intstatus = Convert.ToInt32(DeviceStatus.DeRegistered);
                        db.Entry(deviceObject).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        Success(string.Format("{0} Successfully DeRegistred in server.", mac), true);
                    }
                    else
                    {
                        Warning(string.Format("{0} MacAddress alreadey DeRegister in server", mac), true);
                    }
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chkMacDevice"></param>
        /// <returns></returns>
        public ActionResult ViewDevice(int chkMacDevice)
        {
            try
            {

            }
            catch(Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewMonitorDevices()
        {
            MonitorDevices objMonitorDevice = null;
            try
            {
                this.log.Debug("Enter into the ViewMonitorDevices Action Method");
                EngageLocations objApiCall = new EngageLocations();
                string strResult=objApiCall.GetAllDeviceDetails();
                objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(strResult);
            }
            catch(Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
            }
            return View(objMonitorDevice.records);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLatestPosition(int CompanyId)
        {
            MonitorDevices objMonitorDevice = null;
            this.log.Debug("Enter into the GetLatestPosition Action Method");
            try
            {
                int[] chkMacDevices = (int[]) TempData["MacAddresses"];
                EngageLocations objApiCall = new EngageLocations();
                var objCompanyDetail = db.Company.FirstOrDefault(m => m.CompnayId == CompanyId);
                var objSiteDetail = db.Site.FirstOrDefault(m => m.SiteId ==(int)objCompanyDetail.SiteId);
                string strResult=objApiCall.GetAllDeviceDetailsAsPerSnBn(objCompanyDetail.CompanyName,objSiteDetail.SiteName);
                //string strResult = objtestService.GetLatestPostion(chkMacDevices);
                //objLocationModel = JsonConvert.DeserializeObject<LocationViewModel>(strResult);
                objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(strResult);
                if(objMonitorDevice.result.returncode==0)
                {
                    
                    return View(objMonitorDevice.records);
                }
                else
                {
                    Warning(objMonitorDevice.result.errmsg,false);
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RTLSRegisteredData()
        {
            EngageLocations obj = new EngageLocations();
            obj.GetAllNotification();
            return View();
        }
    }
} 