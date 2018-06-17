using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class MobileClientHandler
    {
        private ILoggingService m_logging;
        private ImageServer m_imageServer;

        public MobileClientHandler(ILoggingService logging, ImageServer imageServer)
        {
            this.m_logging = logging;
            this.m_imageServer = imageServer;
        }

        /// <summary>
        /// handle the client
        /// </summary>
        /// <param name="client">client</param>
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    m_logging.Log("handle mobile client", MessageTypeEnum.INFO);
                }
            }).Start();
        }
    }
}
