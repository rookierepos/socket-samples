using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Tcp.Common;

namespace Tcp.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Connecting to port 8087");

            clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 8087));
            
            _ = PipelinesProcess.ProcessLinesAsync(clientSocket);

            var buffer = new byte[1];
            while (true)
            {
                buffer[0] = (byte)Console.Read();
                await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, 1), SocketFlags.None);
            }
        }
    }
}
