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
using RTLS.ViewModel;

namespace RTLS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationDbContext db = new ApplicationDbContext();
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AdminController));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            List<MacAddress> lstMacAddress = null;
            try
            {
                lstMacAddress = db.MacAddress.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException);
            }
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
        public ActionResult Index(RequestLocationDataVM model)
        {
            try
            {
                using (MacAddressRepository objMacRepository = new MacAddressRepository())
                {
                    objMacRepository.SaveMacAddress(model.MacAddresses, true);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            return RedirectToAction("Index");
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
                string strResult = objApiCall.GetAllDeviceDetails();
                objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(strResult);
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
            }
            return View(objMonitorDevice.records);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public ActionResult RTLSDataAsDevice(int DeviceId)
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RTLSRegisteredData()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRTLSLogs()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chkMacDevices"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteMacAddress(int[] chkMacDevices)
        {
            Result objResult = new Result();
            string retResult = "";
            try
            { 
                this.log.Debug("Enter into the DeleteMacAddress Action Method");
                foreach (var item in chkMacDevices)
                {
                    var deviceObject = db.MacAddress.FirstOrDefault(m => m.Id == item);
                    string mac = deviceObject.Mac;
                    if (deviceObject.Intstatus != Convert.ToInt32(DeviceStatus.Registered))
                    {
                        db.MacAddress.Remove(deviceObject);
                        db.SaveChanges();
                        retResult=string.Format("{0} Successfully Registered into server.", mac);
                    }
                    else
                    {
                        retResult=string.Format("{0} is a Registered User So shouldn't Delete.", mac);

                    }
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.InnerException.Message);
                retResult = "Exception occur" + ex.InnerException.Message;
                objResult.returncode = -1;
            }
            objResult.errmsg = retResult;
            return Json(JsonConvert.SerializeObject(retResult),JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="CompanyName"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult CreateCompany(CompanyViewModel model)
        //{
        //    try
        //    {
        //        Company objCmp = new Company();
        //        if (string.IsNullOrEmpty(model.ddlCompany))
        //        {
        //            objCmp.CompanyName = model.CompanyName;
        //            db.Company.Add(objCmp);
        //        }

        //        db.SaveChanges();

        //        Site objSite = new Site();
        //        objSite.SiteName = model.SiteName;
        //        objSite.CompanyId = objCmp.CompnayId != 0 ? objCmp.CompnayId : Convert.ToInt32(model.ddlCompany);
        //        db.Site.Add(objSite);

        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //    }
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public JsonResult GetCompanyList()
        //{
        //    try
        //    {
        //        var cmpList = db.Company.Select(m => new { text = m.CompanyName, value = m.CompnayId });
        //        return Json(cmpList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        //public ActionResult GetSiteOfCompany(int CompanyId)
        //{
        //    try
        //    {
        //        var siteList = db.Site.Where(m => m.CompanyId == CompanyId).ToList();
        //        return Json(siteList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        throw ex;
        //    }
        //}
    }

    //public class CompanyViewModel
    //{
    //    public string CompanyName { get; set; }
    //    public string SiteName { get; set; }
    //    public string ddlCompany { get; set; }
    //}
}