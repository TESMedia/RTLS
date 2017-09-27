using System.Web.Mvc;
using RTLS.Models;
using log4net;

namespace RTLS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationDbContext db = new ApplicationDbContext();
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(AdminController));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? SiteId)
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public ActionResult RTLSDataAsDevice(string DeviceId)
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RTLSRegisteredData()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRTLSLogs()
        {
            return View();
        }
    }
}