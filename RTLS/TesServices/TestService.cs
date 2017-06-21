using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RTLS.ViewNotification;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace RTLS.Services
{
    public class TestService
    {

        private string strResult;
        private JavaScriptSerializer objSerialization = new JavaScriptSerializer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string AddDeviceMonitor(int [] devices)
        {
            strResult = "{\"result\":{\"returncode\":1,\"errmsg\":\"Unknown device_ids aa:bb:cc:dd:ee:ff,aa:bb:cc:dd:ee:ff\"}" + "}";

            ///var data1 = objSerialization.Deserialize(data);
            //var data1=data["result"];
            return strResult;
           //return strResult;
        }

        public string Authenticate(RTLS.Models.LoginViewModel model)
        {
            strResult = "";

            return strResult;
        }

        /// <summary>
        /// Get the Monitor of these Device
        /// </summary>
        /// <returns></returns>
        public string  DeviceMonitor()
        {
            strResult = "{\"result\":{\"returncode\":\"2011-10-22 14:55:00\",\"errmsg\":\"Tom\"}"+"}";
            
            return strResult;
        }


        /// <summary>
        /// Get the all Area datas for the particualr MacAddress
        /// </summary>
        /// <param name="MacAddressId"></param>
        /// <returns></returns>
        public string GetMonitoredDevice()
        {
            strResult = "{\"records\":[{\"mac\":\"aa: bb: cc: dd: ee: ff\",\"active_count\":99},{\"mac\": \"aa: bb: cc: dd: ee: ff\",\"active_count\":99}],\"result\": {\"returncode\": 0,\"errmsg\":\"\"}" + "}";
            return strResult;
        }


        /// <summary>
        /// Delete the Particular MacAddress
        /// </summary>
        /// <param name="MacAddressId"></param>
        /// <returns></returns>
        public string DeRegisterDevice(int[] devices)
        {
            strResult = "{\"result\":{\"returncode\":1,\"errmsg\":\"Tom\"}" + "}";
            return strResult;
        }


        /// <summary>
        /// return the current location of the named device
        /// </summary>
        /// <returns></returns>
        public string GetLatestPostion(int[] devices)
        {
            strResult = "{\"records\":[{\"mac\":\"aa:bb:cc:dd:ee:ff\",\"active_count\":99,\"sn\": \"some_site\", \"bn\": \"some_building\", \"fn\":\"some_floor\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]},{\"mac\":\"bb:cc:dd:ee:ff:gg\",\"active_count\":90,\"sn\": \"some_sites\", \"bn\": \"some_buildings\", \"fn\":\"some_floors\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]},{\"mac\":\"aa:bb:cc:dd:ee:ff\",\"active_count\":99,\"sn\": \"some_site\", \"bn\": \"some_building\", \"fn\":\"some_floor\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]},{\"mac\":\"aa:bb:cc:dd:ee:ff\",\"active_count\":99,\"sn\": \"some_site\", \"bn\": \"some_building\", \"fn\":\"some_floor\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]},{\"mac\":\"aa:bb:cc:dd:ee:ff\",\"active_count\":99,\"sn\": \"some_site\", \"bn\": \"some_building\", \"fn\":\"some_floor\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]},{\"mac\":\"aa:bb:cc:dd:ee:ff\",\"active_count\":99,\"sn\": \"some_site\", \"bn\": \"some_building\", \"fn\":\"some_floor\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]},{\"mac\":\"aa:bb:cc:dd:ee:ff\",\"active_count\":99,\"sn\": \"some_site\", \"bn\": \"some_building\", \"fn\":\"some_floor\", \"x\": 127, \"y\": 23, \"z\": 0,\"ts_last_seen\": 42343423432, \"an\" :[\"some_area\", \"some_other_area\"]}],\"result\": {\"returncode\": 0,\"errmsg\":\"\"}" + "}";
            return strResult;
        }


        /// <summary>
        /// Get the Device Notification for a MacAddress
        /// </summary>
        /// <returns></returns>
       public string DeviceMontification(int [] devices)
        {
            strResult = "";
            return strResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public string DeleteDevicesMonitor(int [] devices)
        {
            strResult = "{\"result\":{\"returncode\":1,\"errmsg\":\"Tom\"}" + "}";
            return strResult;
        }
    }
}