using RTLS.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTLS.Domins.ViewModels.OmniRequest;
using RTLS.Domins.ViewModels;
using RTLS.Repository;
using RTLS.Common;

namespace RTLS.Business
{
    public class EngageEngineBusiness : IEngageEngineBusiness
    {
        public async Task<bool> regMacToEngageEngine(RequestOmniModel objRequestOmniModel)
        {
            bool result = false;
            EngageRegisterDevice objEngageRegisterDevice = new EngageRegisterDevice();
            using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
            {
                var objSite=objRtlsConfigurationRepository.GetAsPerSite(objRequestOmniModel.SiteId);
                objEngageRegisterDevice.EngageBaseAddressUri = objSite.RtlsConfiguration.EngageBaseAddressUri;
                objEngageRegisterDevice.EngageSiteName = objSite.RtlsConfiguration.EngageSiteName;
                objEngageRegisterDevice.EngageBuildingName = objSite.RtlsConfiguration.EngageBuildingName;
                objEngageRegisterDevice.MacAddress = objRequestOmniModel.MacAddress;
                EngageEngineClient objEngageEngineClient = new EngageEngineClient();
                if(await objEngageEngineClient.RegisterDevice(objEngageRegisterDevice))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
