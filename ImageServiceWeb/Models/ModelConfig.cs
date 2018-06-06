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
    public class ModelConfig
    {
        public event EventHandler Update;

        private static ModelConfig instance;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }

        [Required]
        [Display(Name = "Handlers")]
        public List<string> Handlers { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Connect")]
        public bool Connect { get; set; }

        public string LastHandler { get; set; }

        private TCPClient client;

        public static ModelConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModelConfig();
                }
                return instance;
            }
        }

        private ModelConfig()
        {
            this.LastHandler = "";
            this.Handlers = new List<string>();

            this.client = TCPClient.Instance;
            this.client.MessageReceived += MessageRecivedHandler;
            this.Connect = client.Connect;

            MessageInfo messageInfo = new MessageInfo(CommandEnum.GetConfigCommand, null);
            string message = JsonConvert.SerializeObject(messageInfo);
            client.WriteToServer(message);
        }

        private void MessageRecivedHandler(Object sender, MessageInfo info)
        {
            if (info != null)
            {
                if (info.ID == CommandEnum.GetConfigCommand)
                {
                    ConfigData configData = JsonConvert.DeserializeObject<ConfigData>(info.Args);
                    this.OutputDir = configData.OutputDir;
                    this.SourceName = configData.SourceName;
                    this.LogName = configData.LogName;
                    this.ThumbnailSize = configData.ThumbnailSize;
                    foreach (string handler in configData.Handlers)
                    {
                        this.Handlers.Add(handler);
                    }
                    Update?.Invoke(this, null);
                }

                if (info.ID == CommandEnum.CloseCommand)
                {
                    string handler = info.Args;
                    this.Handlers.Remove(handler);
                    Update?.Invoke(this, null);
                }
            }
        }

        public void DeleteHandler()
        {
            MessageInfo messageInfo = new MessageInfo(CommandEnum.CloseCommand, this.LastHandler);
            string message = JsonConvert.SerializeObject(messageInfo);
            this.client.WriteToServer(message);
        }
    }
}