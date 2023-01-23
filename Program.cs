
using System.Net.Sockets;
using System.Net;

namespace NetworkProgramming_ConsoleIO
{
    public class Program
    {
        public static void FirstTask()
        {
            const string ip_user = "127.0.0.1";
            const int port = 8000;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ip_user);
            IPEndPoint ep = new IPEndPoint(ip, port);
            s.Bind(ep);
            s.Listen(15);
            try
            {
                while (true)
                {
                    Socket ns = s.Accept();
                    Console.WriteLine($"[{ns.RemoteEndPoint.ToString()}]: Hello server!");
                    ns.Send(System.Text.Encoding.Unicode.GetBytes("Hello client!"));
                    Console.ReadKey();
                    ns.Shutdown(SocketShutdown.Both);
                    ns.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Main()
        {
            FirstTask();
        }
    }
}