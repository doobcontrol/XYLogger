using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.xiyuansoft.pub.log
{
    public class LogTask
    {
        public string LogType;
        public static string logType_info = "信息";
        public static string logType_erro = "错误";
        public static string logType_debu = "调试";

        public DateTime LogTime;
        public string LogInfo;
        public System.Exception LogError;
    }
}
