using ImageService.Controller;
using ImageService.Infrastructure.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class CommunicationServer
    {
        private ImageServer m_imageServer;
        private IImageController m_controller;
        private ILoggingService m_logging;

        private int m_port;
        private TcpListener m_listener;
        private IClientHandler m_ch;

        private bool closeCommunication;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="imageServer"></param>
        /// <param name="imageController"></param>
        /// <param name="logging"></param>
        /// <param name="port"></param>
        public CommunicationServer(ImageServer imageServer, IImageController imageController, ILoggingService logging, int port)
        {
            this.m_port = port;
            this.m_logging = logging;
            this.m_controller = imageController;
            this.m_imageServer = imageServer;
            this.m_ch = new ClientHandler(this.m_logging, this.m_imageServer, this.m_controller);
            this.m_logging.MessageRecieved += this.m_ch.SendLogToAllClients;
        }

        /// <summary>
        /// start the server
        /// </summary>
        public void Start()
        {
            // set
            this.closeCommunication = false;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), this.m_port);
            this.m_listener = new TcpListener(ep);

            this.m_listener.Start();
            this.m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() => {
                while (!closeCommunication)
                {
                    try
                    {
                        // accept client
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
        /// close communication
        /// </summary>
        public void CloseCommunication()
        {
            MessageInfo info = new MessageInfo(CommandEnum.CloseServerCommand, null);
            string message = JsonConvert.SerializeObject(info);
            this.m_ch.SendMessageToAllClients(message);
            this.closeCommunication = true;
            this.m_listener.Stop();
        }
    }
}
