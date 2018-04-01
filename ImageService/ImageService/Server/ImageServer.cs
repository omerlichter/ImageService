using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion
        
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
        }

        public void CreateHandler(string directoryPath)
        {
            IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_controller, this.m_logging);
            CommandRecieved += directoryHandler.OnCommandRecieved;
            directoryHandler.StartHandleDirectory(directoryPath);
        }

        public void SendCommand(CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

        public void CloseServer()
        {

        }
    }
}
