using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RTLS.ServiceReturn
{
    public class Notification
    {
        public Notification()
        {
            result = new Result();
        }
        //public static bool IsSuccess { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public int returncode { get; set; }
        public string errmsg { get; set; }
    }

    public static class ServiceResult
    {
        public const int Success = 0;
        public const int Failure = 1;
    }

}