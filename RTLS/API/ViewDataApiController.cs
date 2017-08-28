using log4net;
using Newtonsoft.Json;
using RTLS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("UIData")]
    public class ViewDataApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("ListOfMacAddress")]
        [HttpGet]
        public HttpResponseMessage GetListOfMacAddress()
        {
            PagedResultsViewModel objPagedResults = new PagedResultsViewModel();
            objPagedResults.currentPageIndex=  1;
            objPagedResults.PageRange = 2;

            try
            {
                var Maclist = db.MacAddress.ToList();
                objPagedResults.PageSize = Maclist.Count();
                objPagedResults.TotalPages= (int)Math.Ceiling((decimal)Maclist.Count / (decimal)objPagedResults.PageRange);
                objPagedResults.lstMacAddress.AddRange(Maclist);
                Maclist = Maclist.Skip(((int)objPagedResults.currentPageIndex - 1) * objPagedResults.PageSize).Take(objPagedResults.PageSize).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objPagedResults), Encoding.UTF8, "application/json")
            };
        }

    }
}
