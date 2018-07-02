using log4net;
using RTLS.Domains;
using RTLS.Repository;
using RTLS.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Device")]
    [Authorize]
    public class SaveDeviceApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));
        private ApplicationDbContext db = new ApplicationDbContext();


        [Route("Save")]
        [HttpPost]
        public HttpResponseMessage Save(RequestLocationDataVM model)
        {
            try
            {
                using (MacAddressRepository objMacRepository = new MacAddressRepository())
                {
                    if (objMacRepository.CheckListExistOrNot(model.MacAddresses, model.SiteId))
                    {
                        objMacRepository.SaveMacAddress(model);
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("UpdateDisplay")]
        [HttpPost]
        public HttpResponseMessage UpdateIsDisplay(RequestLocationDataVM model)
        {

            string retResult = "";
            try
            {
               
                if(db.Device.Any(m=>m.MacAddress==model.Mac))
                {
                    var ObjMac = db.DeviceAssociateSite.First(m => m.Device.MacAddress == model.Mac && m.SiteId==model.SiteId && m.IsDeviceRegisterInRtls==true);
                    ObjMac.IsTrackByRtls = model.IsDisplay;
                    db.Entry(ObjMac).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.InnerException.Message);
                retResult = "Exception occur" + ex.InnerException.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("UpdateTrackByAdmin")]
        [HttpPost]
        public HttpResponseMessage UpdateTrackByAdmin(RequestLocationDataVM model)
        {

            string retResult = "";
            try
            {

                if (db.Device.Any(m => m.MacAddress == model.Mac))
                {
                    var ObjMac = db.DeviceAssociateSite.First(m => m.Device.MacAddress == model.Mac && m.SiteId == model.SiteId);
                    ObjMac.IsTrackByAdmin = model.IsTrackByAdmin;
                    db.Entry(ObjMac).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.InnerException.Message);
                retResult = "Exception occur" + ex.InnerException.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [Route("UpdateEntryNotify")]
        [HttpPost]
        public HttpResponseMessage UpdateIsEntryNotify(RequestLocationDataVM model)
        {

            string retResult = "";
            try
            {

                if (db.Device.Any(m => m.MacAddress == model.Mac))
                {
                    var ObjMac = db.DeviceAssociateSite.First(m => m.Device.MacAddress == model.Mac && m.SiteId == model.SiteId);
                    ObjMac.IsEntryNotify = model.IsEntryNotify;
                    db.Entry(ObjMac).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.InnerException.Message);
                retResult = "Exception occur" + ex.InnerException.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
