using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ImageService.Server
{
    interface IClientHandler
    {
        void HandleClient(TcpClient client);
    }
}
