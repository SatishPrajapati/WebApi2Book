using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WebApi2Book.Common.Logging
{
    public class LogManagerAdapter : ILogManager
    {
        public ILog GetLog(Type typeAssociatedWIthRequestedLog)
        {
            var log = LogManager.GetLogger(typeAssociatedWIthRequestedLog);
            return log;
        }
    }
}
