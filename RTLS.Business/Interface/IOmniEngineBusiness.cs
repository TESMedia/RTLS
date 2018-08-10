using RTLS.Domins.ViewModels;
using RTLS.Domins.ViewModels.OmniRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Business.Interface
{
    public interface IOmniEngineBusiness
    {
        Task<ReturnData> regMacToOmniEngine(RequestOmniModel objRequestOmniModel);
    }
}
