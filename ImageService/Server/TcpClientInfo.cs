using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace ImageService.Server
{
    class TcpClientInfo
    {
        // properties
        public TcpClient TcpClient { get; set; }
        public NetworkStream Stream { get; set; }
        public BinaryReader Reader { get; set; }
        public BinaryWriter Writer { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="stream"></param>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        public TcpClientInfo(TcpClient tcpClient, NetworkStream stream, BinaryReader reader, BinaryWriter writer)
        {
            this.TcpClient = tcpClient;
            this.Stream = stream;
            this.Reader = reader;
            this.Writer = writer;
        }
    }
}
