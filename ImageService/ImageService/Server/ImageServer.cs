using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        //private TcpListener m_listener;
        private int m_port;
        private TcpListener m_listener;
        private IClientHandler m_ch;

        private List<TcpClient> m_clientList;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion
        
        /// <summary>
        /// constructor, create handlers for all the directoris in the config file.
        /// </summary>
        /// <param name="controller">controller</param>
        /// <param name="logging">logger</param>
        public ImageServer(IImageController controller, ILoggingService logging, int port)
        {
            this.m_port = port;
            this.m_ch = new ClientHandler();
            this.m_controller = controller;
            this.m_logging = logging;
            this.m_clientList = new List<TcpClient>();
           

            // create handlers for all the directories
            string[] directories = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach(string directoryPath in directories)
            {
                CreateHandler(directoryPath);
            }
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), this.m_port);
            this.m_listener = new TcpListener(ep);

            this.m_listener.Start();
            this.m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = this.m_listener.AcceptTcpClient();
                        this.m_logging.Log("Client Connected", MessageTypeEnum.INFO);
                        this.m_ch.HandleClient(client);
                    }
                    catch (SocketException e)
                    {
                        this.m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                    }
                }
                this.m_logging.Log("Server stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }
    /// <summary>
    /// craete handler to monitoring the directory path.
    /// </summary>
    /// <param name="directoryPath">path to the directory</param>
        public void CreateHandler(string directoryPath)
        {
            // create handler
            IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_controller, this.m_logging);
            // add to events
            CommandRecieved += directoryHandler.OnCommandRecieved;
            directoryHandler.DirectoryClose += this.DeleteHandler;
            // start the handler
            directoryHandler.StartHandleDirectory(directoryPath);
        }

        /// <summary>
        /// send command to all handlers by event.
        /// </summary>
        /// <param name="e">args for the event</param>
        public void SendCommand(CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

        /// <summary>
        /// delete handler and stop the monitoring on the directory.
        /// </summary>
        /// <param name="source">object that send the event</param>
        /// <param name="e">args for the event</param>
        public void DeleteHandler(object source, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler directoryHandler = (IDirectoryHandler)source;
            CommandRecieved -= directoryHandler.OnCommandRecieved;
            directoryHandler.DirectoryClose -= this.DeleteHandler;
            this.m_logging.Log(e.Message, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// send close server to all handlers.
        /// </summary>
        public void CloseServer()
        {
            this.m_listener.Stop();
            CommandRecievedEventArgs commandArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, "*");
            SendCommand(commandArgs);
        }
    }
}
