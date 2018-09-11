using log4net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RTLS.Common;
using RTLS.Domains;
using RTLS.Domins;
using RTLS.Domins.ViewModels;
using RTLS.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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

            IEnumerable<RTLSSetUpReturnDto> OmniMaclist = null;
            IEnumerable EngageMaclist = null;
            try
            {
                if (db.RtlsConfiguration.Any(m => m.SiteId == model.SiteId))
                {
                    OmniMaclist = (from dvcAssociate in db.DeviceAssociateSite.Where(m => m.SiteId == model.SiteId && m.DeviceRegisteredInEngineTypeId != 0)
                                   join wifiCred in db.WifiUserLoginCredential.Where(m => m.SiteId == model.SiteId)
                                   on dvcAssociate.DeviceId equals wifiCred.DeviceId into dvc_Cred_Joined
                                   from wifiCred in dvc_Cred_Joined.DefaultIfEmpty()
                                   select new RTLSSetUpReturnDto
                                   {
                                       Id = dvcAssociate.SiteId,
                                       Mac = dvcAssociate.Device.MacAddress,
                                       omniUniqueId = dvcAssociate.Device.OmniDeviceMapping.UniqueId,
                                       IsTrackByAdmin = dvcAssociate.IsTrackByAdmin,
                                       IsEntryNotify = dvcAssociate.IsEntryNotify,
                                       IsDisplay = dvcAssociate.IsTrackByRtls,
                                       IsCreatedByAdmin = dvcAssociate.IsCreatedByAdmin,
                                       Email = wifiCred.WifiUser.UserName,
                                       FirstName = wifiCred.WifiUser.FirstName,
                                       LastName = wifiCred.WifiUser.LastName,
                                       Status=dvcAssociate.status.ToString()
                                   });

                    TotalOmniRecords = OmniMaclist.Count();

                    OmniMaclist = OmniMaclist.ToList().Skip(SkipStart).Take(FixedLength);

                    foreach (var item in OmniMaclist)
                    {
                        item.FirstName = CommonHasher.Decrypt(item.FirstName, true);
                        item.LastName = CommonHasher.Decrypt(item.LastName, true);
                    }
                }

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
                    OmniMaclist,
                    EngageMaclist
                }), Encoding.UTF8, "application/json")
            };
        }


        [Route("GetLocationData")] 
        [HttpPost]
        public HttpResponseMessage GetListOfLocationData(JQueryDTRequestDeviceData model)
        {
            int FixedLength = Convert.ToInt32(model.RecordToDisply);
            int SkipStart = (Convert.ToInt32(model.CurrentPage) * FixedLength);

            int pages = (SkipStart + FixedLength) / FixedLength;
            int TotalRecords = 0;
            int? timeframe = 0;

            //IEnumerable<LocationData> lstLocationData = null;
            IEnumerable<RtlsNotificationData> lstLocationData = null;
            try
            {
                //var objRtlsConfiguration = db.RtlsConfiguration.FirstOrDefault(m => m.SiteId == model.SiteId);
                //var row = db.LocationData.Where(m => m.sn == objRtlsConfiguration.EngageSiteName); // IsDIsplay =m.IsDeviceRegisterInRtls
                lstLocationData = db.RtlsNotificationData;
                TotalRecords = lstLocationData.Count();
                lstLocationData = db.RtlsNotificationData.OrderByDescending(m => m.NotifyDateTime).ToList().Skip(SkipStart).Take(FixedLength);
                //var _rtlsDataAsPerSite = db.RtlsConfiguration.Where(m => m.SiteId == model.SiteId).FirstOrDefault();
                //timeframe = _rtlsDataAsPerSite.TimeFrame;
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
                    TimeFrame = timeframe,
                    lstLocationData
                }), Encoding.UTF8, "application/json")
            };
        }


        [Route("FilterLocationData")]
        [HttpPost]
        public HttpResponseMessage FilterLocationData(FilterLocationData model)
        {
            int FixedLength = Convert.ToInt32(model.RecordToDisply);
            int SkipStart = (Convert.ToInt32(model.CurrentPage) * FixedLength);
            int pages = (SkipStart + FixedLength) / FixedLength;
            int TotalRecords = 0;
            IEnumerable<RtlsNotificationData> lstLocationData = null;
            try
            {
                //var objRtlsConfiguration = db.RtlsConfiguration.FirstOrDefault(m => m.SiteId == model.SiteId);
                //var row = db.LocationData.Where(m => m.sn == objRtlsConfiguration.EngageSiteName);
                lstLocationData=db.RtlsNotificationData;
                TotalRecords = lstLocationData.Count();
                if (model.MacAddress != null && model.AreaName != null)
                {
                    lstLocationData = db.RtlsNotificationData.OrderByDescending(m => m.NotifyDateTime).Where(m => m.MacAddress == model.MacAddress).Take(FixedLength).ToList();
                }
                else if (model.MacAddress != null)
                {
                    lstLocationData = db.RtlsNotificationData.OrderByDescending(m => m.NotifyDateTime).Where(m => m.MacAddress == model.MacAddress).Take(FixedLength).ToList();
                }
                else if (model.AreaName != null)
                {
                    lstLocationData = db.RtlsNotificationData.OrderByDescending(m => m.NotifyDateTime).Take(FixedLength).ToList();
                }
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
                    RecordToDisply = FixedLength,
                    lstLocationData
                }), Encoding.UTF8, "application/json")
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
