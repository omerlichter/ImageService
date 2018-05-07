using ImageService.Logging.Modal;
using System.Net.Sockets;

namespace ImageService.Server
{
    internal interface IClientHandler
    {
        void HandleClient(TcpClient client);

        void SendLogToAllClients(object sender, MessageRecievedEventArgs args);
        void SendMessageToAllClients(string message);
    }
}