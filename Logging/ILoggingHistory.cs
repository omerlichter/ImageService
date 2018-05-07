using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingHistory
    {
        List<MessageRecievedEventArgs> LogsHistory { get; }

        void AddLog(object sender, MessageRecievedEventArgs args);
    }
}
