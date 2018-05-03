using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Logging;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        private ILoggingService m_logging;
        private List<TcpClient> m_clientList;

        public ClientHandler(ILoggingService logging)
        {
            this.m_logging = logging;
            this.m_clientList = new List<TcpClient>();
        }


        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                this.m_clientList.Add(client);
                bool clientConnect = true;

                while(clientConnect)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        this.m_logging.Log("wait for command from client", MessageTypeEnum.INFO);
                        string commandLine = reader.ReadLine();
                        //Console.WriteLine("Got command: {0}", commandLine);
                        this.m_logging.Log(commandLine, MessageTypeEnum.INFO);
                        writer.Write("recive!");
                    }
                }
                client.Close();
                this.m_clientList.Remove(client);
            }).Start();
        }

        public void SendLogToAllClients(object sender, MessageRecievedEventArgs args)
        {
            new Task(() =>
            {
                foreach (TcpClient client in this.m_clientList)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string message = "2," + args.Message + "," + args.Status;
                        writer.WriteLine(message);
                    }
                }
            }).Start();
        }
    }
}
