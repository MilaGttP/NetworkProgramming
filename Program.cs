
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NetworkProgramming_ConsoleIO
{
    public class QuoteGeneratorClient
    {
        private const int port = 8888;
        private static string ipUser = "127.0.0.1";

        public static void Main()
        {
            Console.WriteLine("Client started...");
            ConnectToServer();
        }

        private static void ConnectToServer()
        {
            IPAddress ip = IPAddress.Parse(ipUser);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(endPoint);
                if (socket.Connected)
                {
                    String strSend = "GET\r\n\r\n";
                    socket.Send(Encoding.Unicode.GetBytes(strSend));
                    byte[] buffer = new byte[1024];
                    int l;
                    do
                    {
                        l = socket.Receive(buffer);
                        Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, l));
                    } while (l > 0);
                }
                else Console.WriteLine("Error");
            }
            catch (SocketException ex) { Console.WriteLine(ex.Message); }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}