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
                    String fileName = GetFileName(stream);
                    Byte[] b = new Byte[1];
                    b[0] = 1;
                    stream.Write(b, 0, 1);
                    byte[] photoArr = GetPhoto(stream);
                    File.WriteAllBytes(m_imageServer.Handlers[0] + "\\" + fileName + ".png", photoArr);
                }
            }).Start();
        }

        public String GetFileName(NetworkStream stream)
        {
            List<Byte> byteList = new List<Byte>();
            Byte[] b = new Byte[1];
            do
            {
                stream.Read(b, 0, 1);
                byteList.Add(b[0]);
            } while (stream.DataAvailable);
            return Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(byteList.ToArray()));
        }

        public byte[] GetPhoto(NetworkStream stream)
        {
            int i = 0;
            List<Byte> byteList = new List<Byte>();
            Byte[] b = new Byte[1];
            do
            {
                i = stream.Read(b, 0, b.Length);
                byteList.Add(b[0]);
            } while (stream.DataAvailable);
            return byteList.ToArray();
        }
    }
}
