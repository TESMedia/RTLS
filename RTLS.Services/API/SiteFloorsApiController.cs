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
using System.Web.Http.Cors;

/// <summary>
/// 
/// </summary>
namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                foreach (var item in ObjRtlsConfig.SiteFloors)
                {
                    if(!_SiteFloorRepoSitory.IsSiteFloorExist(item.Id))
                    {
                        item.RtlsConfigureId = ObjRtlsConfig.SiteId;
                        _SiteFloorRepoSitory.CreateSiteFloor(item);
                    }
                    else
                    {
                        _SiteFloorRepoSitory.UpdateSiteFloor(item);
                    }
                }

                _RrlsConfigurationRepository.SaveAndUpdateAsPerSite(ObjRtlsConfig);

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
               var lstSiteFloor=_SiteFloorRepoSitory.GetAllSiteFloors(SiteId);
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
