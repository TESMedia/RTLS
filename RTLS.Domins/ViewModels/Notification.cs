using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
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
}
