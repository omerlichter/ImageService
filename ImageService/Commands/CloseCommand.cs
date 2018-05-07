using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using ImageService.Infrastructure.Enums;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        private ImageServer m_imageServer;
        
        public CloseCommand(ImageServer imageServer)
        {
            this.m_imageServer = imageServer;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, args[0]);
                this.m_imageServer.SendCommand(command);

                StringBuilder sb = new StringBuilder();
                string[] handlers = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                foreach (string handler in handlers)
                {
                    if (string.Compare(args[0], handler) != 0)
                    {
                        sb.Append(handler);
                        sb.Append(";");
                    }
                }
                ConfigurationManager.AppSettings.Set("Handler", sb.ToString());
                result = true;
                return args[0];
            } catch(Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
