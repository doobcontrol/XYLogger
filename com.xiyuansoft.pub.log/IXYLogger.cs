using System.Collections.Generic;

namespace com.xiyuansoft.pub.log
{
    public interface IXYLogger
    {
        void log(LogTask task);
        void initLog(Dictionary<string, string> initPars);
        void stopLog(Dictionary<string, string> stopPars);

        List<string> readLog(Dictionary<string, string> queryPars);

        void cleanLog(Dictionary<string, string> cleanPars);
    }
}
