using RTLS.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.Repository
{
    public class RtlsConfigurationRepository:IDisposable
    {
        private ApplicationDbContext db = null;
        public RtlsConfigurationRepository()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Site GetAsPerSite(int SiteId,string SiteName)
        {
            return db.Site.Include("RtlsConfiguration").Include("RtlsConfiguration.Devices").FirstOrDefault(m => m.SiteId == SiteId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SaveAndUpdateAsPerSite(RtlsConfiguration model)
        {
            if (CheckRtlsConfigExistOrNotAsPerSite((int)model.SiteId))
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.RtlsConfiguration.Add(model);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool CheckRtlsConfigExistOrNotAsPerSite(int SiteId)
        {
            return db.RtlsConfiguration.Any(m => m.SiteId == SiteId);
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}