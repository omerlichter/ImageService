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
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="imageServer">image server</param>
        public CloseCommand(ImageServer imageServer)
        {
            this.m_imageServer = imageServer;
        }

        /// <summary>
        /// execute the command.
        /// </summary>
        /// <param name="args">args of the command</param>
        /// <param name="result">return the result</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                // send command from image server to close the handler
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, args[0]);
                this.m_imageServer.SendCommand(command);

                // remove the handler from the config file
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
