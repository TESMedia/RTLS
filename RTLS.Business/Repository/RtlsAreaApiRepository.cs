using RTLS.Domains;
using RTLS.Domins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Business.Repository
{
    public class RtlsAreaApiRepository: IDisposable
    {
        private ApplicationDbContext db = null;
        public RtlsAreaApiRepository()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SaveAndUpdateAsPerSite(RtlsArea model)
        {
            if (CheckRtlsAreaExistOrNotAsPerSite((int)model.RtlsConfigurationId))
            {
                int id = db.RtlsArea.FirstOrDefault(m => m.RtlsConfigurationId == model.RtlsConfigurationId).Id;
                model.Id = id;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.RtlsArea.Add(model);
            }
            db.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SaveAndUpdateAsPerSite(List<RtlsArea> RtlsArea)
        {
            db.RtlsArea.AddRange(RtlsArea);
            db.SaveChanges();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void CreateAsPerSite(List<RtlsArea> RtlsArea)
        {
            db.RtlsArea.AddRange(RtlsArea);
            db.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateAsPerSite(List<RtlsArea> RtlsArea)
        {
            foreach (var RtlsAreas in RtlsArea)
            {
                if (RtlsAreas.GeoFencedAreas == null)
                {
                    db.Entry(RtlsAreas).State = System.Data.Entity.EntityState.Deleted;
                }
                else
                {
                    db.Entry(RtlsAreas).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
            //db.RtlsArea.(RtlsArea);
            //db.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RtlsConfigurationId"></param>
        /// <returns></returns>
        public List<RtlsArea> GetListOfGeoFencedAreasPerSite(int RtlsConfigurationId)
        {
            return db.RtlsArea.Include("RtlsConfiguration").Where(m => m.RtlsConfigurationId == RtlsConfigurationId).ToList();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool CheckRtlsAreaExistOrNotAsPerSite(int SiteId)
        {
            return db.RtlsArea.Any(m => m.RtlsConfigurationId == SiteId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool RemoveAreaAsPerSite(int SiteId)
        {
            bool val =false;
            try { 
            if( db.RtlsArea.Any(m => m.RtlsConfigurationId == SiteId))
            {
                db.RtlsArea.RemoveRange(db.RtlsArea.Where(c => c.RtlsConfigurationId == SiteId));
                    db.SaveChanges();
                    val = true;
            }
            else { val = true; }
            }catch(Exception e) { val = false; }
            return val;
        }
        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
