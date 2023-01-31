
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CurrencyRateServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
            tcpListener.Start();

            Console.WriteLine("Currency Rate Server Started...");
            Console.WriteLine("Waiting for a connection...");
            Console.WriteLine();

            while (true)
            {
                Socket socket = tcpListener.AcceptSocket();

                byte[] data = new byte[1024];
                socket.Receive(data);
                string request = Encoding.ASCII.GetString(data);
                double rate = GetExchangeRate(request);
                data = BitConverter.GetBytes(rate);
                socket.Send(data);
                LogRequest(socket.RemoteEndPoint, request, rate);
            }
        }

        static double GetExchangeRate(string request)
        {
            // TODO: Retrieve exchange rate from a database/API
            return 1.14;
        }

        static void LogRequest(EndPoint endPoint, string request, double rate)
        {
            Console.WriteLine("Request from {0}: {1} - {2}", endPoint, request, rate);
        }
    }
}
