﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageService.Infrastructure.Communication
{
    public class LogItem
    {
        public string LogMessage { get; set; }
        public MessageTypeEnum LogType { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public LogItem(MessageTypeEnum type, string message)
        {
            this.LogMessage = message;
            this.LogType = type;
        }
    }
}
