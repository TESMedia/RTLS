using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.Enum
{ 
    public enum DeviceStatus
    {
        None = 0,
        Registered = 1,
        Failed = -1,
        DeRegistered = 2
    }

    public enum RTLSApiResult
    {
        Success = 0,
        Failure = 1
    }
}