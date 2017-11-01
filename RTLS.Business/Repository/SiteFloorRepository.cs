using RTLS.Domains;
using RTLS.Domins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Business.Repository
{
    public class SiteFloorRepository
    {
        private readonly ApplicationDbContext db = null;
        public SiteFloorRepository()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSiteFloors"></param>
        public void CreteSiteFloorsRange (List<SiteFloor> lstSiteFloors)
        {
            db.SiteFloor.AddRange(lstSiteFloors);
            db.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSiteFloor"></param>
        public void CreateSiteFloor(SiteFloor objSiteFloor)
        {
            db.SiteFloor.Add(objSiteFloor);
            db.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSiteFloor"></param>
        public void UpdateSiteFloor(SiteFloor objSiteFloor)
        {
            db.Entry(objSiteFloor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public IQueryable<SiteFloor> GetAllSiteFloors(int SiteId)
        {
           return db.SiteFloor;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteFloorId"></param>
        public void DeleteSiteFloor(int SiteFloorId)
        {
            var objSiteFloor = db.SiteFloor.Find(SiteFloorId);
            db.SiteFloor.Remove(objSiteFloor);
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteFloorId"></param>
        /// <returns></returns>
        public bool IsSiteFloorExist(int SiteFloorId)
        {
           return db.SiteFloor.Any(m=>m.Id== SiteFloorId);
        }
    }
}
