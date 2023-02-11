using com.xiyuansoft.pub.log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace logSample
{
    public partial class FrmlogSample : Form
    {
        public FrmlogSample()
        {
            InitializeComponent(); 
            initLog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log(LogTask.logType_info, " 程序信息输出", null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log(LogTask.logType_debu, " 调试信息输出", null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Control ctr = null;
            try
            {
                ctr.Text = "test";
            }
            catch (NullReferenceException ex)
            {
                log(LogTask.logType_erro, " 未初始化对象引用", ex);
            }
            catch (Exception ex)
            {
                log(LogTask.logType_erro, " 未定异常", ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            log(LogTask.logType_info, " 程序已退出" + "\r\n", null);            
            xyLogger.stopLog(null);

            Close();
        }

        #region 日志

        IXYLogger xyLogger;
        private void initLog()
        {
            xyLogger = new XYFileLogger();
            xyLogger.initLog(new Dictionary<string, string>() {
            {"logDir","log/"},
            {"logFileName","log"}
            });
            log(LogTask.logType_info, " 程序启动", null);
        }
        private void log(string logType, string logInfo, Exception logErro)
        {
            if (xyLogger != null)
            {
                LogTask lt = new LogTask();
                lt.LogType = logType;
                lt.LogInfo = logInfo;
                lt.LogError = logErro;
                lt.LogTime = DateTime.Now;
                xyLogger.log(lt);
            }
        }

        #endregion
    }

}
