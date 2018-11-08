using AspNetCoreMentoring.ConsoleClient.MyNamespace;
using System;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.ConsoleClient
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            var client = new Client("http://localhost:56786");
           var  products = await client.GetAllAsync(0,10);
            Console.WriteLine("Hello World!");
        }
    }
}
