using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Client
{
    class TCPClient
    {
        public event EventHandler<string> MessageReceived;

        private static readonly TCPClient instance = new TCPClient();

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
                TcpClient client = new TcpClient();
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
                this.writer.Write(str);
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
                        Console.WriteLine("start reading from server...");
                        string message = this.reader.ReadString();
                        Console.WriteLine("end reading");
                        this.MessageReceived?.Invoke(this, message);
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
            this.stream.Close();
            this.reader.Close();
            this.writer.Close();
        }
    }
}
