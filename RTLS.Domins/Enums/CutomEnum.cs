using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.Enums
{
    public enum DeviceStatus
    {
        None = 0,
        Registered = 1,
        Failed = -1,
        DeRegistered = 2
    }

    public enum FatiApiResult
    {
        Success = 0,
        Failure = 1
    }
}
