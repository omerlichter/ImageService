using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class MobileServer
    {
        private ImageServer m_imageServer;
        private IImageController m_controller;
        private ILoggingService m_logging;

        private int m_port;
        private TcpListener m_listener;
        private MobileClientHandler m_ch;

        private bool closeCommunication;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="imageServer"></param>
        /// <param name="imageController"></param>
        /// <param name="logging"></param>
        /// <param name="port"></param>
        public MobileServer(ImageServer imageServer, IImageController imageController, ILoggingService logging, int port)
        {
            this.m_port = port;
            this.m_logging = logging;
            this.m_controller = imageController;
            this.m_imageServer = imageServer;
            this.m_ch = new MobileClientHandler(this.m_logging, this.m_imageServer);
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
            m_logging.Log("start mobile server", MessageTypeEnum.INFO);

            Task task = new Task(() => {
                while (!closeCommunication)
                {
                    try
                    {
                        // accept client
                        TcpClient client = this.m_listener.AcceptTcpClient();
                        this.m_ch.HandleClient(client);
                        m_logging.Log("start communication with mobile....", MessageTypeEnum.INFO);
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
            this.closeCommunication = true;
            this.m_listener.Stop();
        }
    }
}
