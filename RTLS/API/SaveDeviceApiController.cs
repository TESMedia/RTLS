using log4net;
using RTLS.Models;
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
                    if (objMacRepository.CheckListExistOrNot(model.MacAddresses))
                    {
                        objMacRepository.SaveMacAddress(model.MacAddresses, true);
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
               
                if(db.MacAddress.Any(m=>m.Mac==model.Mac))
                {
                    var ObjMac = db.MacAddress.First(m => m.Mac == model.Mac);
                    ObjMac.IsDisplay = model.IsDisplay;
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
