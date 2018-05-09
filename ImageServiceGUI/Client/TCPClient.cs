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
using ImageService.Infrastructure.Enums;

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

        public bool Connect { get { return this.isConnect; } }

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

        /// <summary>
        /// constructor
        /// </summary>
        private TCPClient() {
            this.isConnect = StartCommunication();
        }

        /// <summary>
        /// start communication with server
        /// </summary>
        /// <returns>true if connect</returns>
        private bool StartCommunication()
        {
            try
            {
                // set
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                this.client = new TcpClient();
                // connect
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

        /// <summary>
        /// write string to the server
        /// </summary>
        /// <param name="str"></param>
        public void WriteToServer(string str)
        {
            if (this.Connect)
            {
                try
                {
                    Console.WriteLine("write to server...");
                    writer.Write(str);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// read from server all the time, and notify when new message sent
        /// </summary>
        private void ReadFromServer()
        {
            new Task(() =>
            {
                while (!endCommunication)
                {
                    try
                    {
                        // read message
                        string message = reader.ReadString();
                        Console.WriteLine("reading from server: " + message);
                        MessageInfo info = JsonConvert.DeserializeObject<MessageInfo>(message);

                        if (info.ID == CommandEnum.CloseServerCommand)
                        {
                            CloseCommunication();
                            break;
                        }

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

        /// <summary>
        /// close the communication
        /// </summary>
        public void CloseCommunication()
        {
            try
            {
                endCommunication = true;
                this.writer.Close();
                this.reader.Close();
                this.stream.Close();
                this.client.Close();
            } catch
            {

            }
        }
    }
}
