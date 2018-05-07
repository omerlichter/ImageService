﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Communication;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    class GetLogHistoryCommand : ICommand
    {
        private ILoggingHistory m_loggingHistory;

        public GetLogHistoryCommand(ILoggingHistory loggingHistory)
        {
            this.m_loggingHistory = loggingHistory;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<LogItem> logsList = new List<LogItem>();
                foreach(MessageRecievedEventArgs messageArgs in this.m_loggingHistory.LogsHistory)
                {
                    LogItem logItem = new LogItem(messageArgs.Status, messageArgs.Message);
                    logsList.Add(logItem);
                }
                LogData logData = new LogData(logsList);
                string logsHistoryString = JsonConvert.SerializeObject(logData);
                result = true;
                return logsHistoryString;
            } catch(Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
