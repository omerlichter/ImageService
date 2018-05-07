using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingHistory : ILoggingHistory
    {
        private List<MessageRecievedEventArgs> logsHistory = new List<MessageRecievedEventArgs>();

        public List<MessageRecievedEventArgs> LogsHistory {
            get { return this.logsHistory; }
        }

        public void AddLog(object sender, MessageRecievedEventArgs args)
        {
            this.logsHistory.Add(args);
        }
    }
}
