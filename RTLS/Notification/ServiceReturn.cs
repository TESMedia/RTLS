using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RTLS.ServiceReturn
{
    public class Notification
    {
        //public static bool IsSuccess { get; set; }
        public Result result { get;}
    }

    public class Result
    {
        public int returncode { get; }
        public string errmsg { get; }
    }

    public static class ServiceResult
    {
        public const int Success = 0;
        public const int Failure = 1;
    }
   
}