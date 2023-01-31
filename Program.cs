
using System;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CurrencyRateClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Currency Rate Client Started...");
            Console.WriteLine("Enter two currencies (e.g. USD EUR):");
            string currencies = Console.ReadLine();
            TcpClient tcpClient = new TcpClient("localhost", 8888);
            tcpClient.Connect("127.0.0.1", 8888);
            byte[] data = Encoding.ASCII.GetBytes(currencies);
            tcpClient.GetStream().Write(data, 0, data.Length);
            data = new byte[1024];
            int bytes = tcpClient.GetStream().Read(data, 0, data.Length);
            double rate = BitConverter.ToDouble(data, 0);
            Console.WriteLine("Exchange rate: {0}", rate);
        }
    }
}
