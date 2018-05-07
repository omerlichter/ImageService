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

        private static TCPClient instance;

        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        private bool endCommunication;
        private bool isConnect;

        private TCPClient() {
            this.isConnect = StartCommunication();
        }

        public static TCPClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TCPClient();
                }
                return instance;
            }
        }

        public bool Connect { get { return this.isConnect; } }

        private bool StartCommunication()
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
            isConnect = true;
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
                while (!endCommunication)
                {
                    try
                    {
                        string message = reader.ReadString();
                        Console.WriteLine("reading from server: " + message);

                        if (string.Compare(message, "close") == 0)
                        {
                            CloseCommunication();
                            break;
                        }

                        MessageInfo info = JsonConvert.DeserializeObject<MessageInfo>(message);
                        this.MessageReceived?.Invoke(this, info);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
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
