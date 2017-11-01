using RTLS.Business.Repository;
using RTLS.Domains;
using RTLS.Domins;
using RTLS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

/// <summary>
/// 
/// </summary>
namespace RTLS.API
{
    [RoutePrefix("api")]
    public class SiteFloorsApiController : ApiController
    {
        private  SiteFloorRepository _SiteFloorRepoSitory { get; }
        private RtlsConfigurationRepository _RrlsConfigurationRepository { get; }
        /// <summary>
        /// 
        /// </summary>
        public SiteFloorsApiController()
        {
            _SiteFloorRepoSitory = new SiteFloorRepository();
            _RrlsConfigurationRepository = new RtlsConfigurationRepository();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSiteFloors"></param>
        /// <returns></returns>
        [Route("SaveSiteFloors")]
        [HttpPost]
        public HttpResponseMessage SaveSiteFloors(RtlsConfiguration ObjRtlsConfig)
        {
            try
            {
                RtlsConfiguration objRtlsConfiguration = null;
                if (_RrlsConfigurationRepository.CheckRtlsConfigExistOrNotAsPerSite(ObjRtlsConfig.SiteId, ObjRtlsConfig.SiteName))
                {

                    objRtlsConfiguration = _RrlsConfigurationRepository.GetAsPerSite(ObjRtlsConfig.SiteId, ObjRtlsConfig.SiteName);
                }
                else
                {
                    _RrlsConfigurationRepository.SaveAndUpdateAsPerSite(ObjRtlsConfig);
                }

                foreach(var item in ObjRtlsConfig.SiteFloors)
                {
                    if(!_SiteFloorRepoSitory.IsSiteFloorExist(item.Id))
                    {
                        item.RtlsConfigureId = objRtlsConfiguration.RtlsConfigurationId;
                        _SiteFloorRepoSitory.CreateSiteFloor(item);
                    }
                    else
                    {
                        item.RtlsConfigureId = objRtlsConfiguration.RtlsConfigurationId;
                        _SiteFloorRepoSitory.UpdateSiteFloor(item);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [Route("GetSiteFloors")]
        [HttpGet]
        public HttpResponseMessage GetSiteFloors(int SiteId)
        {
            try
            {
               var lstSiteFloor=_SiteFloorRepoSitory.GetAllSiteFloors(SiteId).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, lstSiteFloor);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [Route("DeleteSiteFloor")]
        [HttpDelete]
        public HttpResponseMessage DeleteSiteFloor(SiteFloor objSie)
        {
            try
            {
                _SiteFloorRepoSitory.DeleteSiteFloor(objSie.Id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
