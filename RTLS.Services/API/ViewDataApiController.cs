using log4net;
using Newtonsoft.Json;
using RTLS.Domains;
using RTLS.Domins.ViewModels;
using RTLS.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("UIData")]
    [Authorize]
    public class ViewDataApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("ListOfMacAddress")]
        [HttpPost]
        public HttpResponseMessage GetListOfMacAddress(RequestLocationDataVM model)
        {
            IEnumerable Maclist=null;
            try
            {
                if(db.RtlsConfiguration.Any(m=>m.SiteId== model.SiteId))
                {
                    Maclist = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId).Select(m=>new {Id=m.Id, Mac=m.Device.MacAddress, StrStatus=m.status.ToString(),  IsTrackByAdmin = m.IsTrackByAdmin,IsDisplay = m.IsTrackByRtls ,m.IsCreatedByAdmin}).ToList(); // IsDIsplay =m.IsDeviceRegisterInRtls
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(Maclist), Encoding.UTF8, "application/json")
            };
        }

        [Route("AjaxListOfMacAddress")]
        [HttpPost]
        public HttpResponseMessage AjaxGetListOfMacAddress(JQueryDTRequestDeviceData model)
        {
            int FixedLength = Convert.ToInt32(model.RecordToDisply);
            int SkipStart = (Convert.ToInt32(model.CurrentPage)* FixedLength);
           
            int pages = (SkipStart + FixedLength) / FixedLength;
            int TotalRecords = 0;
            
            IEnumerable  Maclist = null;
            try
            {
                if (db.RtlsConfiguration.Any(m => m.SiteId == model.SiteId))
                {
                    var row = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId && m.IsDeviceRegisterInRtls==true).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin}).ToList(); // IsDIsplay =m.IsDeviceRegisterInRtls

                    TotalRecords = row.Count;
                    
                }
                var DeviceAssociateSite = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId && m.IsDeviceRegisterInRtls==true).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin }).ToList().Skip(SkipStart).Take(FixedLength);


                //var displayLocationData = DeviceAssociateSite;
                Maclist = from c in DeviceAssociateSite
                          select new { Id = c.Id, Mac = c.Mac, Status = c.StrStatus, IsTrackByAdmin = c.IsTrackByAdmin, IsDisplay = c.IsDisplay, IsCreatedByAdmin = c.IsCreatedByAdmin };
                
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    CurrentPage = pages,
                    TotalRecords = TotalRecords,
                    RecordToDisply = FixedLength,
                    Maclist }), Encoding.UTF8, "application/json")
            };
        }
    }


}
