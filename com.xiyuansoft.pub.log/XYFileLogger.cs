using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.xiyuansoft.pub.log
{
    /// <summary>
    /// 文件日志类
    /// </summary>
    public class XYFileLogger : IXYLogger
    {
        #region IXYLogger Members

        void IXYLogger.log(LogTask task)
        {
            logTaskList.Add(task);

            if (logThread == null)
            {
                logThread = new System.Threading.Thread(new System.Threading.ThreadStart(logFun));
                logThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                logThread.Start();
            }
            else if (!logThread.IsAlive)
            {
                logThread.Abort();
                logThread = new System.Threading.Thread(new System.Threading.ThreadStart(logFun));
                logThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                logThread.Start();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="initPars">必需含 logDir（日志目录） logFileName（日志文件名前缀，实际名称会加上日期）值</param>
        void IXYLogger.initLog(Dictionary<string, string> initPars)
        {
            logDir = initPars["logDir"];
            if (!Directory.Exists(logDir))//如果不存在就创建file文件夹　　             　　              
                Directory.CreateDirectory(logDir);//创建该文件夹　　
            logFileName = initPars["logFileName"];

            if (logThread == null)
            {
                logThread = new System.Threading.Thread(new System.Threading.ThreadStart(logFun));
                logThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                logThread.Start();
            }
        }

        void IXYLogger.stopLog(Dictionary<string, string> stopPars)
        {
            if (logThread != null)
            {
                Thread stopThread = new System.Threading.Thread(new System.Threading.ThreadStart(stopRun));
                //stopThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                stopThread.Start();
            }
        }

        List<string> IXYLogger.readLog(Dictionary<string, string> queryPars)
        {
            return null;//暂不实现
        }

        void IXYLogger.cleanLog(Dictionary<string, string> cleanPars)
        {
            IEnumerable<string> fileEm= Directory.EnumerateFiles(logDir);
            DateTime delDate = DateTime.Now.AddMonths(-3);  //删除三个月前的全部日志
            foreach (string fielname in fileEm)
            {
                DateTime fileDate = File.GetCreationTime(fielname);
                if (fileDate < delDate)
                {
                    try
                    {
                        File.Delete(fielname);
                    }
                    catch (Exception e)
                    {
                        LogTask lt = new LogTask();
                        lt.LogType = LogTask.logType_erro;
                        lt.LogInfo = "清理日志错误:" + fielname;
                        lt.LogError = e;
                        lt.LogTime = DateTime.Now;
                        (this as IXYLogger).log(lt);
                    }
                }
            }
        }

        #endregion

        ~XYFileLogger()
        {
            stopRun();
        }


        private List<LogTask> logTaskList = new List<LogTask>();
        private string logDir;
        private string logFileName;

        private Thread logThread;
        private bool stopLog = false;
        private void logFun()
        {
            string fileName = "";
            StreamWriter log = null;
            try
            {
                while (!stopLog)
                {
                    while (logTaskList.Count > 0)
                    {
                        LogTask lt = logTaskList[0];
                        //如果时间跨天，重新生成日志文件
                        string newfileName =
                            logDir +
                            logFileName.Split('.')[0] +
                            lt.LogTime.ToString("yyyyMMdd") +
                            ".log";
                        if (newfileName != fileName)
                        {
                            fileName = newfileName;
                            if (log != null)
                            {
                                log.Flush();
                                log.Close();
                            }
                            if (!File.Exists(fileName))
                            {
                                log = new StreamWriter(fileName);
                            }
                            else
                            {
                                log = File.AppendText(fileName);
                            }
                        }

                        log.WriteLine(lt.LogTime + " -- " + lt.LogType + " -- " + lt.LogInfo);
                        if (lt.LogError != null)
                        {
                            log.WriteLine(lt.LogError.Message);
                            log.WriteLine(lt.LogError.StackTrace);
                        }
                        log.Flush();

                        logTaskList.Remove(lt);
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (System.Threading.ThreadAbortException e)
            {
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (log != null)
                {
                    log.Flush();
                    log.Close();
                }
                logTaskList.Clear();
            }
        }
        private void stopRun()
        {
            if (logThread != null)
            {
                while (logTaskList.Count > 0) ;

                stopLog = true;
                logThread.Join();
            }
        }
    
    }
}
