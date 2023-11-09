namespace ServerMessenger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Server server = new Server(12345);
            Console.WriteLine("server start");
            await server.StartAsync();
            Console.WriteLine("seerver stoped");
        }
    }
}