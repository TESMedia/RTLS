﻿using RTLS.Domains;
using RTLS.Domins.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RTLS.Controllers
{
    public class BaseController : Controller, IDisposable
    {
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            try
            {
                int SkipStart = Convert.ToInt32(Request["start"]);
                int FixedLength = Convert.ToInt32(Request["length"]);
                int pages = (SkipStart + FixedLength) / FixedLength;
                int TotalRecords = 0;
                IEnumerable<LocationData> filteredLocationData = null;
                param.sSearch = Request["search[value]"].ToString();
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    TotalRecords = db.LocationData.Count();
                    var allLocationData = db.LocationData.ToList().Skip(SkipStart).Take(FixedLength);
                    if (!string.IsNullOrEmpty(param.sSearch))
                    {
                        filteredLocationData = db.LocationData.ToList().Where(c => c.AreaName.Contains(param.sSearch) || c.bn.Contains(param.sSearch) || c.mac.Contains(param.sSearch));

                    }
                    else
                    {
                        filteredLocationData = allLocationData;
                    }

                    var displayLocationData = filteredLocationData;
                    var result = from c in displayLocationData
                                 select new { Id = c.Id, mac = c.mac, sn = c.sn, bn = c.bn, fn = c.fn, x = c.x, y = c.y, z = c.z, last_seen_ts = c.last_seen_ts, AreaName = c.AreaName };

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = TotalRecords,
                        iTotalDisplayRecords = TotalRecords,
                        aaData = result
                    },
                      JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxHandlerToGetLogs(jQueryDataTableParamModel param)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var allLocationData = db.Database.SqlQuery<AppLog>("GetAllAppLog").ToList();
                    IEnumerable<AppLog> filteredLocationData;
                    if (!string.IsNullOrEmpty(param.sSearch))
                    {
                        filteredLocationData = allLocationData.ToList().Where(c => c.Date == Convert.ToDateTime(param.sSearch));
                    }
                    else
                    {
                        filteredLocationData = allLocationData;
                    }

                    var displayLocationData = filteredLocationData;
                    var result = from c in displayLocationData
                                 select new { Id = c.Id, Date = c.Date.ToShortDateString(), Thread = c.Thread, Level = c.Level, Logger = c.Logger, Message = c.Message, Exception = c.Exception };

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = allLocationData.Count(),
                        iTotalDisplayRecords = allLocationData.Count(),
                        aaData = result
                    },
                      JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult AjaxHandlerGetDeviceData(string DeviceId)
        {
            try
            {
                int SkipStart = Convert.ToInt32(Request["start"]);
                int FixedLength = Convert.ToInt32(Request["length"]);
                int pages = (SkipStart + FixedLength) / FixedLength;
                int TotalRecords = 0;
                IEnumerable<LocationData> filteredLocationData = null;
                string sSearch = Request["search[value]"].ToString();
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    TotalRecords = db.LocationData.Where(m => m.mac == DeviceId).Count();
                    var allLocationData = db.LocationData.Where(m => m.mac == DeviceId).ToList().Skip(SkipStart).Take(FixedLength);
                    if (!string.IsNullOrEmpty(sSearch))
                    {
                        filteredLocationData = allLocationData.Where(c => c.AreaName.ToLower().Contains(sSearch.ToLower()));
                    }
                    else
                    {
                        filteredLocationData = allLocationData;
                    }

                    var displayLocationData = filteredLocationData;
                    var result = from c in displayLocationData
                                 select new { Id = c.Id, mac = c.mac, sn = c.sn, bn = c.bn, fn = c.fn, x = c.x, y = c.y, z = c.z, last_seen_ts = c.last_seen_ts, AreaName = c.AreaName };

                    return Json(new
                    {
                        sEcho = "",
                        iTotalRecords = TotalRecords,
                        iTotalDisplayRecords = TotalRecords,
                        aaData = result
                    },
                      JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Sjy Todo Remove
        //public JsonResult AjaxHandlerGetMacAddress(JQueryDTRequestDeviceData param)
        //{
        //    try
        //    {
        //        int SkipStart = Convert.ToInt32(Request["start"]);
        //        int FixedLength = Convert.ToInt32(Request["length"]);
        //        int pages = (SkipStart + FixedLength) / FixedLength;
        //        int TotalRecords = 0;
        //        IEnumerable MacList = null;


        //        param.sSearch = Request["search[value]"].ToString();
        //        using (ApplicationDbContext db = new ApplicationDbContext())
        //        {

        //            if (db.RtlsConfiguration.Any(m => m.SiteId == param.SiteId))
        //            {
        //                var t = db.DeviceAssociateSite.Where(m => m.Site.SiteId == param.SiteId).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin }).ToList(); // IsDIsplay =m.IsDeviceRegisterInRtls
        //                TotalRecords = t.Count;
        //                MacList = t;
        //            }

        //            //TotalRecords = MacList.     //db.LocationData.Count();
        //            var allLocationData = db.DeviceAssociateSite.Where(m => m.Site.SiteId == param.SiteId).Select(m => new { Id = m.Id, Mac = m.Device.MacAddress, StrStatus = m.status.ToString(), IsTrackByAdmin = m.IsTrackByAdmin, IsDisplay = m.IsTrackByRtls, m.IsCreatedByAdmin }).ToList().Skip(SkipStart).Take(FixedLength);


        //            var displayLocationData = allLocationData;
        //            var result = from c in allLocationData
        //                         select new { Id = c.Id, Mac = c.Mac, Status = c.StrStatus, IsTrackByAdmin = c.IsTrackByAdmin, IsDisplay = c.IsDisplay, IsCreatedByAdmin = c.IsCreatedByAdmin };

        //            return Json(new
        //            {
        //                sEcho = param.sEcho,
        //                iTotalRecords = TotalRecords,
        //                iTotalDisplayRecords = TotalRecords,
        //                aaData = result
        //            },
        //              JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}