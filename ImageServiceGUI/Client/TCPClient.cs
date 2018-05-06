using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ImageService.Infrastructure.Communication;

namespace ImageServiceGUI.Client
{
    class TCPClient
    {
        public event EventHandler<MessageInfo> MessageReceived;

        private static readonly TCPClient instance = new TCPClient();

        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        private bool endCommunication;

        private TCPClient() { }

        public static TCPClient Instance
        {
            get { return instance; }
        }

        public bool StartCommunication()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                this.client = new TcpClient();
                client.Connect(ep);
                this.stream = client.GetStream();
                this.reader = new BinaryReader(stream);
                this.writer = new BinaryWriter(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            endCommunication = false;
            ReadFromServer();
            return true;
        }

        public void WriteToServer(string str)
        {
            try
            {
                Console.WriteLine("write to server...");
                writer.Write(str);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ReadFromServer()
        {
            new Task(() =>
            {
                try
                {
                    while (!endCommunication)
                    {
                        try
                        {
                            string message = reader.ReadString();
                            Console.WriteLine("reading from server: " + message);
                            MessageInfo info = JsonConvert.DeserializeObject<MessageInfo>(message);
                            this.MessageReceived?.Invoke(this, info);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                    }
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }).Start();
        }

        public void CloseCommunication()
        {
            endCommunication = true;
            this.writer.Close();
            this.reader.Close();
            this.stream.Close();
            this.client.Close();
        }
    }
}
