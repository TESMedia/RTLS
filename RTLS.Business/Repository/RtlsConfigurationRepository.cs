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
        public RtlsConfiguration GetAsPerSite(int SiteId,string SiteName)
        {
           if( CheckRtlsConfigExistOrNotAsPerSite(SiteId,SiteName))
            {
                return db.RtlsConfigurations.FirstOrDefault(m => m.SiteId == SiteId || m.SiteName== SiteName);
            }
           else
            {
                return new RtlsConfiguration();
            }
           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SaveAndUpdateAsPerSite(RtlsConfiguration model)
        {
            if (CheckRtlsConfigExistOrNotAsPerSite((int)model.SiteId,model.SiteName))
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.RtlsConfigurations.Add(model);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool CheckRtlsConfigExistOrNotAsPerSite(int SiteId,string SiteName)
        {
            return db.RtlsConfigurations.Any(m => m.SiteId == SiteId || m.SiteName==SiteName);
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}