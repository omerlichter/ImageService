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

        /// <summary>
        /// add log to history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void AddLog(object sender, MessageRecievedEventArgs args)
        {
            this.logsHistory.Add(args);
        }
    }
}
