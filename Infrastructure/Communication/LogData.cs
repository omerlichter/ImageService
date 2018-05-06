using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Communication
{
    public class LogData
    {
        public List<LogItem> LogsList { get; set; }

        public LogData(List<LogItem> logs)
        {
            this.LogsList = logs;
        }
    }
}
