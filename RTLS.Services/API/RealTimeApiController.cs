using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using RTLS.ViewModel;
using RTLS.Repository;
using Newtonsoft.Json;
using RTLS.ReturnModel;
using log4net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;
using RTLS.Domins.ViewModels;
using RTLS.Domains;
using RTLS.Domins.Enums;
using RTLS.Domins.ViewModels.Request;
using System.Data.Entity;
using RTLS.Business.Repository;
using RTLS.Business;
using RTLS.Domins.ViewModels.OmniRequest;
using RTLS.Common;
using Newtonsoft.Json.Linq;

namespace RTLS.API
{
    [RoutePrefix("RealTimeLocation")]
    public class RealTimeApiController : ApiController
    {
        private HttpClient httpClient = null;
        private string queryParams = null;
        private string completeFatiAPI = ConfigurationManager.AppSettings["FatiDeviceApi"].ToString();
        private RtlsConfigurationRepository objRtlsConfigurationRepository = null;
        private static log4net.ILog Log { get; set; }
        OmniDeviceMappingRepository _OmniDeviceMappingRepository = new OmniDeviceMappingRepository();
        ILog log = log4net.LogManager.GetLogger(typeof(RealTimeApiController));
        private ApplicationDbContext db = new ApplicationDbContext();

        public RealTimeApiController()
        {
            objRtlsConfigurationRepository = new RtlsConfigurationRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void CommonHeaderInitializeHttpClient(string EngageBaseUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(EngageBaseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]))));
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddDevices")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddDevices(RequestLocationDataVM model)
        {
            Notification objNotifications = new Notification();
            DeviceAssociateSite deviceid = null;
            using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
            {
                Site objSite = objRtlsConfigurationRepository.GetAsPerSite(model.SiteId);

                foreach (var item in model.MacAddresses)
                {
                    // When Device is coming for reregister in OmniEngiene
                    int deviceId = _OmniDeviceMappingRepository.GetDeviceId(item);
                     deviceid = objRtlsConfigurationRepository.DeviceAssociateSiteStatus(deviceId);
                    if (deviceid.status == DeviceStatus.DeRegistered)
                    {
              
                            OmniEngineBusiness objOmniEngineBusiness = new OmniEngineBusiness();
                            RequestOmniModel objRequestOmniModel = new RequestOmniModel();
                            objRequestOmniModel.MacAddress = item;
                            await objOmniEngineBusiness.ReRegister(objRequestOmniModel);
                     }


                }
                //First time devive will store
                if (deviceid.status == DeviceStatus.None)
                {
                    try
                    {
                        if(objSite.RtlsConfiguration.RtlsEngineType == RtlsEngine.OmniEngine)
                        {
                            foreach (var item in model.MacAddresses)
                            {
                                OmniEngineBusiness objOmniEngineBusiness = new OmniEngineBusiness();
                                RequestOmniModel objRequestOmniModel = new RequestOmniModel();
                                objRequestOmniModel.MacAddress = item;
                                var retrnResult = await objOmniEngineBusiness.regMacToOmniEngine(objRequestOmniModel);
                                if (retrnResult.Status == true)
                                {
                                    objNotifications.result.returncode = Convert.ToInt32(FatiApiResult.Success);
                                    using (MacAddressRepository objMacRepository = new MacAddressRepository())
                                    {
                                        objMacRepository.RegisterListOfMacAddresses(model);
                                    }
                                }
                            }
                        }
                        if (objSite.RtlsConfiguration.RtlsEngineType == RtlsEngine.EngageEngine)
                        {
                            foreach (var item in model.MacAddresses)
                            {
                                EngageEngineBusiness objEngageEngineBusiness = new EngageEngineBusiness();
                                RequestOmniModel objRequestOmniModel = new RequestOmniModel();
                                objRequestOmniModel.SiteId = model.SiteId;
                                objRequestOmniModel.MacAddress = item;
                                if (await objEngageEngineBusiness.regMacToEngageEngine(objRequestOmniModel))
                                {
                                    objNotifications.result.returncode = Convert.ToInt32(FatiApiResult.Success);
                                    using (MacAddressRepository objMacRepository = new MacAddressRepository())
                                    {
                                        objMacRepository.RegisterListOfMacAddresses(model);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.InnerException.Message);
                        objNotifications.result.returncode = -1;
                        objNotifications.result.errmsg = ex.InnerException.Message;
                    }


                }
            }                
                return new HttpResponseMessage()
                {
                   
                    Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
                };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteDevices")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteDevices(RequestLocationDataVM model)
        {

            Notification objNotifications = new Notification();
            DeviceAssociateSite deviceid = null;
            using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
            {
                Site objSite = objRtlsConfigurationRepository.GetAsPerSite(model.SiteId);

                foreach (var item in model.MacAddresses)
                {
                    // When Device is coming for reregister in OmniEngiene
                    int deviceId = _OmniDeviceMappingRepository.GetDeviceId(item);
                    deviceid = objRtlsConfigurationRepository.DeviceAssociateSiteStatus(deviceId);
                    if (deviceid.status == DeviceStatus.Registered || deviceid.status == DeviceStatus.DeRegistered)
                    {

                        OmniEngineBusiness objOmniEngineBusiness = new OmniEngineBusiness();
                        RequestOmniModel objRequestOmniModel = new RequestOmniModel();
                        objRequestOmniModel.MacAddress = item;
                        var returnStatus=await objOmniEngineBusiness.DeleteDevices(objRequestOmniModel);
                        if(returnStatus==true)
                        {
                            Device _objDevice = db.Device.FirstOrDefault(m=>m.MacAddress==item);
                            db.Device.Remove(_objDevice);
                            db.SaveChanges();

                        }
                    }


                }
            }

             return new HttpResponseMessage()
            {

                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetDevices")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDevices(RequestLocationDataVM model)
        {
             MonitorDevices objMonitorDevice = new MonitorDevices();

            //Notification objNotifications = new Notification();
            using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
            {
                Site objSite = objRtlsConfigurationRepository.GetAsPerSite(model.SiteId, model.SiteName);
                CommonHeaderInitializeHttpClient(objSite.RtlsConfiguration.EngageBaseAddressUri);

                //Check the Parameter search or not,if it then add in the QueryParams,else keep as it is
                queryParams = new FormUrlEncodedContent(new Dictionary<string, string>()
                 {
                    { "sn",objSite.RtlsConfiguration.EngageSiteName },
                    { "bn",objSite.RtlsConfiguration.EngageBuildingName}
                    //,
                    //{"device_ids",String.Join(",",model.MacAddresses) }
                 }).ReadAsStringAsync().Result;
                completeFatiAPI = objSite.RtlsConfiguration.EngageBaseAddressUri + "?" + queryParams;
                try
                {
                    //var result = await httpClient.GetAsync(completeFatiAPI, new StringContent(queryParams, Encoding.UTF8, "application/x-www-form-urlencoded"));
                    var result = await httpClient.GetAsync(completeFatiAPI);
                    if (result.IsSuccessStatusCode)
                    {

                       String msg = "";
                        
                        string resultContent = await result.Content.ReadAsStringAsync();
                        objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(resultContent);
                        using (MacAddressRepository objMacRepo = new MacAddressRepository())
                        {
                            msg = objMacRepo.CheckDeviceRegisted(objMonitorDevice, model.SiteId);

                        }
                        objMonitorDevice.result.errmsg = msg;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objMonitorDevice), Encoding.UTF8, "application/json")
                //Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeRegisterDevices")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeRegisterDevices(RequestLocationDataVM model)
        {
            Notification objNotifications = new Notification();
            HttpRequestMessage message = new HttpRequestMessage();
            OmniEngineBusiness objOmniEngineBusiness = new OmniEngineBusiness();
            RequestOmniModel objRequestOmniModel = new RequestOmniModel();
            try
            {
                using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
                {
                    Site objSite = objRtlsConfigurationRepository.GetAsPerSite(model.SiteId, model.SiteName);
                    if(objSite.RtlsConfiguration.RtlsEngineType == RtlsEngine.OmniEngine)
                    {
                        foreach (var item in model.MacAddresses)
                        {
                            
                            objRequestOmniModel.MacAddress = item;
                            var deregisterData= await objOmniEngineBusiness.DeregisterMacFromOmniEngine(objRequestOmniModel);

                        }

                    }
                    else if(objSite.RtlsConfiguration.RtlsEngineType == RtlsEngine.EngageEngine)
                    {
                        CommonHeaderInitializeHttpClient(objSite.RtlsConfiguration.EngageBaseAddressUri);
                        message = new HttpRequestMessage(new HttpMethod("DELETE"), "/api/engage/v1/device_monitors/");
                        var queryParams = new Dictionary<string, string>()
                      {
                        { "sn",objSite.RtlsConfiguration.EngageSiteName },
                        { "bn",objSite.RtlsConfiguration.EngageBuildingName },
                        {"device_ids",String.Join(",",model.MacAddresses) }
                      };

                        message.Content = new FormUrlEncodedContent(queryParams);
                        try
                        {
                            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(message);
                            if (httpResponseMessage.EnsureSuccessStatusCode().IsSuccessStatusCode)
                            {
                                string resultContent = await httpResponseMessage.Content.ReadAsStringAsync();
                                objNotifications = JsonConvert.DeserializeObject<Notification>(resultContent);
                                if (objNotifications.result.returncode == Convert.ToInt32(FatiApiResult.Success))
                                {
                                    using (MacAddressRepository objMacRepository = new MacAddressRepository())
                                    {
                                        objMacRepository.DeRegisterListOfMacs(model.MacAddresses);
                                    }
                                }
                            }
                            else
                            {
                                objNotifications.result.returncode = Convert.ToInt32(httpResponseMessage.StatusCode.ToString());
                                objNotifications.result.errmsg = "Some Problem Occured";
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorType = ex.GetType().ToString();
                            string errorMessage = errorType + ": " + ex.Message;
                            throw new Exception(errorMessage, ex.InnerException);
                        }
                    }
                   
                   
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
                objNotifications.result.returncode = -1;
                objNotifications.result.errmsg = ex.InnerException.Message;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }


        [Route("TestMemeberApplication")]
        [HttpPost]
        public HttpResponseMessage TestMemeberApplicationPost(LocationData objLocationData)
        {
            log.Debug("Eneter into the TestMemeberApplicationPost");
            Notification objNotifications = new Notification();
            try
            {
                //RecieveDateTime
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    TrackMember objTrackMember = new TrackMember();
                    objTrackMember.MacAddress = objLocationData.mac;
                    objTrackMember.VisitedDateTime = objLocationData.LastSeenDatetime;
                    objTrackMember.PostDateTime = objLocationData.PostDateTime;
                    objTrackMember.RecieveDateTime = DateTime.Now;
                    objTrackMember.AreaName = objLocationData.an[0].ToString();
                    objTrackMember.X = objLocationData.x;
                    objTrackMember.Y = objLocationData.y;
                    db.TrackMember.Add(objTrackMember);
                    db.SaveChanges();
                    objNotifications.result.returncode = Convert.ToInt32(FatiApiResult.Success);
                    objNotifications.result.errmsg = "SuccessFully Created";
                }
            }
            catch (Exception ex)
            {
                objNotifications.result.returncode = Convert.ToInt32(FatiApiResult.Failure);
                objNotifications.result.errmsg = "Error Occured";
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }


        [Route("StartNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> StartNotification(NotificationRequest model)
        {
            Notification objNotifications = new Notification();
            
           
            try { 
            #region Save ResetTime in RtlsConfiguration

            if (model.NotificationType != null && model.ResetTime > 0)
            {
                //save notification Reset Time
                RtlsConfiguration objrtls = objRtlsConfigurationRepository.GetAsPerSiteId(model.SiteId);
                if (objrtls != null)
                {
                        if (model.NotificationType == NotificationType.Approach) objrtls.AreaNotification = model.ResetTime; 
                        if (model.NotificationType == NotificationType.Entry) objrtls.ApproachNotification = model.ResetTime;
                        objRtlsConfigurationRepository.SaveAndUpdateAsPerSite(objrtls);
                    }
            }
            #endregion

            #region Save Notification Type in DeviceAssociateSite
            if (model.NotificationType != null && model.MacAddressList!=null && model.MacAddressList.Length > 0)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    foreach (var mac in model.MacAddressList)
                    {
                        if (db.Device.Any(m => m.MacAddress == mac))
                        {
                            var ObjMacNotify = db.DeviceAssociateSite.First(m => m.Device.MacAddress == mac && m.SiteId == model.SiteId);
                                if (model.NotificationType == NotificationType.Approach)
                                    ObjMacNotify.IsTrackByAdmin = true;
                                else if (model.NotificationType == NotificationType.Entry)
                                    ObjMacNotify.IsEntryNotify = true;
                                else if (model.NotificationType== NotificationType.All) { ObjMacNotify.IsTrackByAdmin = true; ObjMacNotify.IsEntryNotify = true; }
                            db.Entry(ObjMacNotify).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            #endregion

            #region Save Area in RtlsArea
            if (model.AreaID != null && model.AreaID.Length > 0)
            {
                //RtlsConfiguration objrtls = objRtlsConfigurationRepository.GetAsPerSiteId(model.SiteId);
                List<RtlsArea> arealist = new List<RtlsArea>();
                foreach (var area in model.AreaID)
                {
                    RtlsArea a = new RtlsArea();
                    a.GeoFencedAreas = area;
                    a.RtlsConfigurationId = model.SiteId;
                    arealist.Add(a);
                }
                if (arealist.Count > 0)
                {
                    using (RtlsAreaApiRepository rtlsRepo = new RtlsAreaApiRepository())
                    {
                            rtlsRepo.RemoveAreaAsPerSite(model.SiteId);
                            rtlsRepo.SaveAndUpdateAsPerSite(arealist);
                    }
                }
            }
            #endregion

            #region Register MAC Address to RTLS
            if (model.MacAddressList.Length > 0)
            {
                    RequestLocationDataVM rldvm = new RequestLocationDataVM();
                    rldvm.SiteId = model.SiteId;
                    string[] RegisteredMacAdress; List<string> lstDeviceToRegister = new List<string>();
                    using(MacAddressRepository macRepo = new MacAddressRepository())
                    {
                        RegisteredMacAdress = macRepo.GetMacByStatus(DeviceStatus.Registered);
                    }
                    if (RegisteredMacAdress != null & RegisteredMacAdress.Length > 0)
                    {
                        foreach(var mac in model.MacAddressList)
                        {
                            if (!RegisteredMacAdress.Contains(mac))
                                lstDeviceToRegister.Add(mac);
                        }
                    }
                    else { lstDeviceToRegister.AddRange(model.MacAddressList); }
                    
                   rldvm.SiteName = "";
                   rldvm.MacAddresses = lstDeviceToRegister.ToArray();
                    try
                    {
                        await AddDevices(rldvm);
                    }
                    catch
                    {
                        //TODO Possible duplicate mac address error.
                    }
                }
                #endregion

                objNotifications.result.returncode = 0;
                objNotifications.result.errmsg = "";
            }
            catch(Exception e)
            {
                objNotifications.result.returncode = -1;
                objNotifications.result.errmsg = e.Message;

            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }

        [Route("StopNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> StopNotification(NotificationRequest model)
        {
            Notification objNotifications = new Notification();
            bool isAreaNotificationExist = false;
            RequestLocationDataVM rldvm = new RequestLocationDataVM();
            try
            {
                #region Remove ResetTime in RtlsConfiguration

                //Remove notification Reset Time
                //TODO Check with Jon- if we need to remove the site level configuration.
                RtlsConfiguration objrtls = objRtlsConfigurationRepository.GetAsPerSiteId(model.SiteId);
                if (objrtls != null)
                {
                    if (model.NotificationType == NotificationType.Approach)
                        objrtls.AreaNotification = 0; 
                    else if (model.NotificationType == NotificationType.Entry)
                        objrtls.ApproachNotification = 0;
                    else if (model.NotificationType == NotificationType.All)
                    {
                        objrtls.ApproachNotification = 0;
                        objrtls.AreaNotification = 0;
                    }
                    objRtlsConfigurationRepository.SaveAndUpdateAsPerSite(objrtls);
                }

                #endregion
               

                #region Remove Notification Type in DeviceAssociateSite
                if (model.NotificationType != null && model.MacAddressList != null && model.MacAddressList.Length > 0)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        foreach (var mac in model.MacAddressList)
                        {
                            if (db.Device.Any(m => m.MacAddress == mac))
                            {
                                var ObjMacNotify = db.DeviceAssociateSite.First(m => m.Device.MacAddress == mac && m.SiteId == model.SiteId);
                                if (model.NotificationType == NotificationType.Approach)
                                    ObjMacNotify.IsTrackByAdmin = false;
                                else if (model.NotificationType == NotificationType.Entry)
                                    ObjMacNotify.IsEntryNotify = false;
                                else if(model.NotificationType == NotificationType.All)
                                {
                                    ObjMacNotify.IsTrackByAdmin = false; ObjMacNotify.IsEntryNotify = false;
                                }
                                db.Entry(ObjMacNotify).State = EntityState.Modified;
                                db.SaveChanges();
                                
                                #region check if this Mac adress have any of notification enabled.
                                if(ObjMacNotify.IsTrackByAdmin || ObjMacNotify.IsEntryNotify) { 
                                    List<string> list = new List<string>(model.MacAddressList);
                                    list.Remove(mac);
                                    model.MacAddressList = list.ToArray();
                                }
                                if (ObjMacNotify.IsTrackByAdmin) isAreaNotificationExist = true;
                                #endregion
                            }
                        }
                    }
                }
                #endregion

                #region Remove Area in RtlsArea
                //TODO Check with Jon If need to remove site level configuration
                using (RtlsAreaApiRepository rtlsRepo = new RtlsAreaApiRepository())
                {
                    if (model.NotificationType == NotificationType.Approach && !isAreaNotificationExist)
                         rtlsRepo.RemoveAreaAsPerSite(model.SiteId);
                }
                
                #endregion

                #region De-Register MAC Address to RTLS
                if (model.MacAddressList.Length > 0)
                {
                    rldvm.SiteId = model.SiteId;
                    string[] RegisteredMacAdress; List<string> lstDeviceToRegister = new List<string>();
                    using (MacAddressRepository macRepo = new MacAddressRepository())
                    {
                        RegisteredMacAdress = macRepo.GetMacByStatus(DeviceStatus.None);
                    }
                    if (RegisteredMacAdress != null & RegisteredMacAdress.Length > 0)
                    {
                        foreach (var mac in model.MacAddressList)
                        {
                            if (!RegisteredMacAdress.Contains(mac))
                                lstDeviceToRegister.Add(mac);
                        }
                    }
                    else { lstDeviceToRegister.AddRange(model.MacAddressList); }

                    rldvm.SiteName = "";
                    rldvm.MacAddresses = lstDeviceToRegister.ToArray();
                        try
                        {
                            await DeRegisterDevices(rldvm);
                        }
                        catch
                        {
                            //TODO Possible duplicate mac address error.
                        }
                    

                }
                #endregion

                objNotifications.result.returncode = 0;
                objNotifications.result.errmsg = "Success";
            }
            catch (Exception e)
            {
                objNotifications.result.returncode = -1;
                objNotifications.result.errmsg = e.Message;

            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }

        [Route("UserNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> UserNotification(NotificationRequest model)
        {
            Notification objNotifications = new Notification();

            RequestLocationDataVM rldvm = new RequestLocationDataVM();
            try
            {
                #region Save Notification Type in DeviceAssociateSite
                if (model.NotificationType != null && model.MacAddressList != null && model.MacAddressList.Length > 0)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        foreach (var mac in model.MacAddressList)
                        {
                            if (db.Device.Any(m => m.MacAddress == mac))
                            {
                                var ObjMacNotify = db.DeviceAssociateSite.First(m => m.Device.MacAddress == mac && m.SiteId == model.SiteId);
                                if (model.NotificationType == NotificationType.Approach)
                                    ObjMacNotify.IsTrackByAdmin = true;
                                else if (model.NotificationType == NotificationType.Entry)
                                    ObjMacNotify.IsEntryNotify = true;
                                else if (model.NotificationType == NotificationType.All)
                                {
                                    ObjMacNotify.IsTrackByAdmin = true; ObjMacNotify.IsEntryNotify = true;
                                }
                                db.Entry(ObjMacNotify).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                #endregion
                
                #region Register MAC Address to RTLS
                if (model.MacAddressList.Length > 0)
                {
                    string[] RegisteredMacAdress; List<string> lstDeviceToRegister = new List<string>();
                    using (MacAddressRepository macRepo = new MacAddressRepository())
                    {
                        RegisteredMacAdress = macRepo.GetMacByStatus(DeviceStatus.Registered);
                    }
                    if (RegisteredMacAdress != null & RegisteredMacAdress.Length > 0)
                    {
                        foreach (var mac in model.MacAddressList)
                        {
                            if (!RegisteredMacAdress.Contains(mac))
                                lstDeviceToRegister.Add(mac);
                        }
                    }
                    else { lstDeviceToRegister.AddRange(model.MacAddressList); }

                    rldvm.SiteName = "";
                    rldvm.MacAddresses = lstDeviceToRegister.ToArray();
                    rldvm.SiteId = model.SiteId;
                    try
                        {
                            await AddDevices(rldvm);
                        }
                        catch
                        {
                            //TODO Possible duplicate mac address error.
                        }
                   
                }
                #endregion

                objNotifications.result.returncode = 0;
                objNotifications.result.errmsg = "Success";
            }
            catch (Exception e)
            {
                objNotifications.result.returncode = -1;
                objNotifications.result.errmsg = e.Message;

            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objNotifications), Encoding.UTF8, "application/json")
            };
        }
    }

}

