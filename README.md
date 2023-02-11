使用示例代码：<br>
            IXYLogger xyLogger;<br>
            xyLogger = new XYFileLogger();<br>
            xyLogger.initLog(new Dictionary<string, string>() {<br>
            {"logDir","log/"},<br>
            {"logFileName","log"}<br>
            });<br>
            log(LogTask.logType_info, " 程序启动", null)<br>

initLog参数字典中，logDir值是日志的保存位置，logFileName是日志的文件名，实际生成的文件名为加上日期，为“logyymmdd.log”
实际使用请见示例项目
 
