using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";

            Server.Start(50, 42050);
            
            Console.ReadKey();
        }
    }
}
