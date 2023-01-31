
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace RecipeServer
{
    class Program
    {
        static List<string> recipes = new List<string>();
        static void Main(string[] args)
        {
            recipes.Add("Grilled Cheese Sandwich: 2 slices of bread, butter, 1 slice of cheese");
            recipes.Add("Strawberry Smoothie: 2 cups of strawberries, 1 cup of milk, 1 tablespoon of honey");
            recipes.Add("Baked Salmon: 2 salmon fillets, 2 tablespoons of olive oil, 2 cloves of garlic");
            recipes.Add("Veggie Stir Fry: 1 onion, 1 red pepper, 1 green pepper, 1 cup of mushrooms, 2 tablespoons of soy sauce");

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 8080);
            socket.Bind(localEndpoint);

            Console.WriteLine("Recipe Server started. Waiting for client requests...");
            EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 8080);
            byte[] buffer = new byte[1024];

            while (true)
            {
                int receivedBytes = socket.ReceiveFrom(buffer, ref remoteEndpoint);
                string request = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                string[] products = request.Split(',');
                string response = GetRecipes(products);
                byte[] responseBuffer = System.Text.Encoding.ASCII.GetBytes(response);
                socket.SendTo(responseBuffer, remoteEndpoint);

                Console.WriteLine("Sent response to " + remoteEndpoint);
            }
        }

        static string GetRecipes(string[] products)
        {
            List<string> results = new List<string>();
            foreach (string recipe in recipes)
            {
                bool containsAll = true;
                foreach (string product in products)
                {
                    if (!recipe.Contains(product))
                    {
                        containsAll = false;
                        break;
                    }
                }
                if (containsAll) results.Add(recipe);
            }
            return string.Join(",", results);
        }
    }
}