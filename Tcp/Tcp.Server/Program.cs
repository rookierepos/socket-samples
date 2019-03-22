using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tcp.Common;

namespace Tcp.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8087));

            Console.WriteLine("Listening on port 8087");

            listenSocket.Listen(120);

            ConcurrentBag<Socket> clients = new ConcurrentBag<Socket>();

            _ = Task.Run(async() =>
            {
                while (true)
                {
                    var clientSocket = await listenSocket.AcceptAsync();
                    clients.Add(clientSocket);
                    _ = PipelinesProcess.ProcessLinesAsync(clientSocket);
                }
            });
            var buffer = new byte[1];
            while (true)
            {
                buffer[0] = (byte) Console.Read();
                foreach (var item in clients)
                {
                    await item.SendAsync(new ArraySegment<byte>(buffer, 0, 1), SocketFlags.None);
                }
            }
        }
    }
}
