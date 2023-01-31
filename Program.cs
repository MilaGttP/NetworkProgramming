
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkProgramming_ConsoleIO
{
    public class QuoteGeneratorServer
    {
        private const int port = 8888;
        private static List<string> quotes = new List<string>();
        private static Dictionary<string, DateTime> connections = new Dictionary<string, DateTime>();

        public static void Main()
        {
            Console.WriteLine("Server started...");
            InitializeQuotes();
            StartListening();
        }

        private static void InitializeQuotes()
        {
            quotes.Add("The only way to do great work is to love what you do. - Steve Jobs");
            quotes.Add("If you can dream it, you can do it. - Walt Disney");
            quotes.Add("Success is not final; failure is not fatal: It is the courage to continue that counts. - Winston Churchill");
            quotes.Add("Don't watch the clock; do what it does. Keep going. - Sam Levenson");
            quotes.Add("The best way to get started is to quit talking and begin doing. - Walt Disney");
        }

        private static void StartListening()
        {
            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection...");

                    Socket clientSocket = listener.Accept();
                    string connectionID = Guid.NewGuid().ToString();
                    connections.Add(connectionID, DateTime.Now);

                    Console.WriteLine($"Connection accepted. ConnectionID: {connectionID}");

                    byte[] bytes = new byte[1024];
                    int bytesRec = clientSocket.Receive(bytes);
                    string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    while (data.ToLower() != "exit")
                    {
                        Random rnd = new Random();
                        int index = rnd.Next(quotes.Count);
                        string quote = quotes[index];

                        byte[] message = Encoding.UTF8.GetBytes(quote);
                        clientSocket.Send(message);

                        bytesRec = clientSocket.Receive(bytes);
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    }

                    Console.WriteLine($"Connection closed. ConnectionID: {connectionID}. Time connected: {connections[connectionID]}");
                    connections.Remove(connectionID);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

}