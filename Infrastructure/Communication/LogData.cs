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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logs"></param>
        public LogData(List<LogItem> logs)
        {
            this.LogsList = logs;
        }
    }
}
