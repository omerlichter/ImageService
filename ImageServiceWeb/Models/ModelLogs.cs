using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ImageServiceWeb.Client;
using ImageService.Infrastructure.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;

namespace ImageServiceWeb.Models
{
    public class ModelLogs
    {
        public event EventHandler Update;

        [Required]
        [Display(Name = "Logs")]
        public List<LogItem> Logs { get; set; }

        public ModelLogs()
        {
            TCPClient client = TCPClient.Instance;
            client.MessageReceived += MessageRecivedHandler;

            this.Logs = new List<LogItem>();
            MessageInfo messageInfo = new MessageInfo(CommandEnum.LogCommand, null);
            string message = JsonConvert.SerializeObject(messageInfo);
            client.WriteToServer(message);
        }

        private void MessageRecivedHandler(Object sender, MessageInfo info)
        {
            if (info != null)
            {
                if (info.ID == CommandEnum.LogCommand)
                {
                    LogData logsData = JsonConvert.DeserializeObject<LogData>(info.Args);
                    foreach (LogItem item in logsData.LogsList)
                    this.Logs.Add(item);
                    Update?.Invoke(this, null);
                }
            }
        }
    }
}