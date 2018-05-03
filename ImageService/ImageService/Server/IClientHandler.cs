using System.Net.Sockets;

namespace ImageService.Server
{
    internal interface IClientHandler
    {
        void HandleClient(TcpClient client);
    }
}