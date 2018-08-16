using log4net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RTLS.Domains;
using RTLS.Domins;
using RTLS.Domins.ViewModels;
using RTLS.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
        private MySqlConnection connection;
        string query = null;
        string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        [Route("ListOfMacAddress")]
        [HttpPost]
        public HttpResponseMessage GetListOfMacAddress(RequestLocationDataVM model)
        {
            IEnumerable Maclist = null;
            try
            {
                if (db.RtlsConfiguration.Any(m => m.SiteId == model.SiteId))
                {
                    Maclist = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId && m.IsDeviceRegisterInRtls == true).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin }).ToList(); // IsDIsplay =m.IsDeviceRegisterInRtls
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
            int SkipStart = (Convert.ToInt32(model.CurrentPage) * FixedLength);

            int pages = (SkipStart + FixedLength) / FixedLength;
            int TotalRecords = 0;
            int TotalOmniRecords = 0;
            int TotalEngageRecords = 0;

            IEnumerable  OmniMaclist = null;
            IEnumerable EngageMaclist = null;
            try
            {
                if (db.RtlsConfiguration.Any(m => m.SiteId == model.SiteId))
                {
                    var row = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsEntryNotify=m.IsEntryNotify, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin }).ToList(); // IsDIsplay =m.IsDeviceRegisterInRtls

                    TotalRecords = row.Count;
                    
                }
                var DeviceAssociateSite = db.DeviceAssociateSite.Where(m => m.Site.SiteId == model.SiteId).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsEntryNotify = m.IsEntryNotify, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin }).ToList().Skip(SkipStart).Take(FixedLength);

                //var displayLocationData = DeviceAssociateSite;
                Maclist = from c in DeviceAssociateSite
                          select new { Id = c.Id, Mac = c.Mac, Status = c.StrStatus, IsTrackByAdmin = c.IsTrackByAdmin, IsDisplay = c.IsDisplay, IsCreatedByAdmin = c.IsCreatedByAdmin, IsEntryNotify=c.IsEntryNotify };
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    CurrentPage = pages,
                    TotalRecords = TotalRecords,
                    TotalOmniRecords= TotalOmniRecords,
                    TotalEngageRecords=TotalEngageRecords,
                    RecordToDisply = FixedLength,
                    Maclist }), Encoding.UTF8, "application/json")
            };
        }




        [Route("UpdateRTLSDataDelete")]
        [HttpPost]
        public HttpResponseMessage UpdateRTLSDataDelete(FilterLocationData model)
        {
            var _rtlsDataAccordingtoSite = db.RtlsConfiguration.Where(m => m.SiteId == model.SiteId).FirstOrDefault();
            _rtlsDataAccordingtoSite.TimeFrame = model.TimeFrame;
            db.SaveChanges();
            // ConnectionString
            connection = new MySqlConnection(ConnectionString);
            if (model.TimeFrame != 0 && model.TimeFrame != 1)
            {
                query = "DROP EVENT IF EXISTS ClearRTLSData; CREATE EVENT ClearRTLSData ON SCHEDULE EVERY " + " " + model.TimeFrame + " " + " HOUR COMMENT 'Clear RTLS Data as per Admin Configuration' DO CALL ArchiveRTLSData(" + "'" + _rtlsDataAccordingtoSite.EngageSiteName + "'" + ")";

            }
            else
            {
                query = "DROP EVENT IF EXISTS ClearRTLSData";
            }

            //open connection
            connection.Open();
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            cmd.ExecuteNonQuery();

            //close connection
            connection.Close();

            return new HttpResponseMessage()
            {

            };
        }

    }
}
