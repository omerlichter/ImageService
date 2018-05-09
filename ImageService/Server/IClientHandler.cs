using ImageService.Logging.Modal;
using System.Net.Sockets;

namespace ImageService.Server
{
    internal interface IClientHandler
    {
        /// <summary>
        /// handle the client
        /// </summary>
        /// <param name="client">client</param>
        void HandleClient(TcpClient client);

        /// <summary>
        /// send log to all clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void SendLogToAllClients(object sender, MessageRecievedEventArgs args);

        /// <summary>
        /// send message to all client
        /// </summary>
        /// <param name="message"></param>
        void SendMessageToAllClients(string message);
    }
}