using RTLS.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Business.Repository
{
    public class NetworkConfigurationRepository : IDisposable
    {
        private ApplicationDbContext db = null;
        public NetworkConfigurationRepository()
        {
            db = new ApplicationDbContext();
        }
        public bool CheckSelfExcusionExistOrNotAsPerSite(int SiteId) 
        {
            var _objNetwork = db.NetWorkOfSite.Where(p => p.SiteId == SiteId).Where(q => q.LocServiceTypeId != 0).FirstOrDefault();
            if(_objNetwork==null)
            {
                return false;
            }


            return true;
        }
        public void Dispose() 
        {
            ((IDisposable)db).Dispose();
        }
    } 
}
