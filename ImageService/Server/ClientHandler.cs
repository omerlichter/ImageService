using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ImageService.Logging.Modal;
using ImageService.Logging;
using Newtonsoft.Json;
using ImageService.Infrastructure.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Controller;
using ImageService.Modal;
using System.Threading;

namespace ImageService.Server
{
    class ClientHandler : IClientHandler
    {
        private static Mutex writerMut = new Mutex();

        private ILoggingService m_logging;
        private ImageServer m_imageServer;
        private IImageController m_controller;
        private List<TcpClientInfo> m_clientList;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="imageServer"></param>
        /// <param name="controller"></param>
        public ClientHandler(ILoggingService logging, ImageServer imageServer, IImageController controller)
        {
            this.m_logging = logging;
            this.m_imageServer = imageServer;
            this.m_controller = controller;
            this.m_clientList = new List<TcpClientInfo>();
        }

        /// <summary>
        /// handle the client
        /// </summary>
        /// <param name="client">client</param>
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                TcpClientInfo clientInfo = new TcpClientInfo(client, stream, reader, writer);
                this.m_clientList.Add(clientInfo);
                bool clientConnect = true;
                this.m_logging.Log("num of connected clients: " + this.m_clientList.Count, MessageTypeEnum.INFO);

                try
                {
                    while (clientConnect)
                    {
                        // read message from the server
                        string commandLine = reader.ReadString();
                        MessageInfo info = JsonConvert.DeserializeObject<MessageInfo>(commandLine);

                        // if the command is CloseGUICommand
                        if (info.ID == CommandEnum.CloseGUICommand)
                        {
                            clientConnect = false;
                            this.m_clientList.Remove(clientInfo);
                            client.Close();
                            this.m_logging.Log("Client DisConnected", MessageTypeEnum.INFO);
                            return;
                        }
                        // if the command is CloseCommand
                        if (info.ID == CommandEnum.CloseCommand)
                        {
                            bool result;
                            string[] executeArgs = { info.Args };
                            string value = this.m_controller.ExecuteCommand((int)info.ID, executeArgs, out result);


                            // send back
                            MessageInfo messageBack = null;
                            if (result)
                            {
                                messageBack = new MessageInfo(info.ID, value);
                            }
                            else
                            {
                                messageBack = new MessageInfo(CommandEnum.FailCommand, null);
                            }
                            string messageBackString = JsonConvert.SerializeObject(messageBack);
                            this.SendMessageToAllClients(messageBackString);
                        }
                        else
                        {
                            // execute the command
                            bool result;
                            string[] executeArgs = { info.Args };
                            string value = this.m_controller.ExecuteCommand((int)info.ID, executeArgs, out result);

                            // send back
                            MessageInfo messageBack = null;
                            if (result)
                            {
                                messageBack = new MessageInfo(info.ID, value);
                            }
                            else
                            {
                                messageBack = new MessageInfo(CommandEnum.FailCommand, null);
                            }
                            string messageBackString = JsonConvert.SerializeObject(messageBack);
                            writerMut.WaitOne();
                            writer.Write(messageBackString);
                            writerMut.ReleaseMutex();
                        }
                    }
                } catch(Exception e)
                {
                    clientConnect = false;
                    client.Close();
                    this.m_clientList.Remove(clientInfo);
                    return;
                }
                clientConnect = false;
                client.Close();
                this.m_clientList.Remove(clientInfo);
            }).Start();
        }

        /// <summary>
        /// send log to all clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SendLogToAllClients(object sender, MessageRecievedEventArgs args)
        {
            new Task(() =>
            {
                LogItem logItem = new LogItem(args.Status, args.Message);
                List<LogItem> logList = new List<LogItem>();
                logList.Add(logItem);
                LogData logData = new LogData(logList);
                string serializeArgs = JsonConvert.SerializeObject(logData);
                MessageInfo info = new MessageInfo(CommandEnum.LogCommand, serializeArgs);
                string message = JsonConvert.SerializeObject(info);

                // send the log to all connected clients
                foreach (TcpClientInfo clientInfo in this.m_clientList)
                {
                    try
                    {
                        writerMut.WaitOne();
                        clientInfo.Writer.Write(message);
                        writerMut.ReleaseMutex();
                    } catch(Exception e)
                    {
                        this.m_clientList.Remove(clientInfo);
                        clientInfo.TcpClient.Close();
                    }
                }
            }).Start();
        }

        /// <summary>
        /// send message to all client
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageToAllClients(string message)
        {
            new Task(() =>
            {
                // send message to all connected clients
                foreach (TcpClientInfo clientInfo in this.m_clientList)
                {
                    try
                    {
                        writerMut.WaitOne();
                        clientInfo.Writer.Write(message);
                        writerMut.ReleaseMutex();
                    }
                    catch (Exception e)
                    {
                        this.m_clientList.Remove(clientInfo);
                        clientInfo.TcpClient.Close();
                    }
                }
            }).Start();
        }
    }
}
