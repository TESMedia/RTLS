using log4net;
using Newtonsoft.Json;
using RTLS.Enum;
using RTLS.Models;
using RTLS.ServiceReturn;
using RTLS.ViewModel;
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
    [RoutePrefix("Device")]
    public class DeleteDeviceApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete(RequestLocationDataVM model)
        {
            Result objResult = new Result();
            string retResult = "";
            try
            {
                this.log.Debug("Enter into the DeleteMacAddress Action Method");
                foreach (var item in model.MacAddresses)
                {
                    var deviceObject = db.MacAddress.FirstOrDefault(m => m.Mac == item);
                    if (deviceObject.Intstatus != Convert.ToInt32(DeviceStatus.Registered))
                    {
                        db.MacAddress.Remove(deviceObject);
                        db.SaveChanges();
                        retResult = string.Format("{0} Successfully Deleted from server", item);
                    }
                    else
                    {
                        retResult = string.Format("{0} is a Registered User So shouldn't Delete", item);

                    }
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.InnerException.Message);
                retResult = "Exception occur" + ex.InnerException.Message;
                objResult.returncode = -1;
            }
            objResult.errmsg = retResult;
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(retResult))
            };
        }
    }
}
