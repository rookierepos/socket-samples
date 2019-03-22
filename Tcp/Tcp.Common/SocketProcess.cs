using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tcp.Common
{
    public class SocketProcess
    {
        public static async Task ProcessLinesAsync(Socket socket)
        {
            Console.WriteLine($"[{socket.RemoteEndPoint}]: connected");

            using (var stream = new NetworkStream(socket))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    ProcessLine(socket, await reader.ReadLineAsync());
                }
            }

            Console.WriteLine($"[{socket.RemoteEndPoint}]: disconnected");
        }

        private static void ProcessLine(Socket socket, string s)
        {
            Console.Write($"[{socket.RemoteEndPoint}]: ");
            Console.WriteLine(s);
        }
    }
}
