using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateService
{
   public class NtsLog
    {
        public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(Exception ex)
        {
            if (ex.InnerException != null)
            {
                Logger.Error(ex.InnerException);
            }
            else
            {
                Logger.Error(ex);
            }
        }
    }
}
