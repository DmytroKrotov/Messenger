using ClientMessenger.CommunicationItems;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

public class Client
{
    private TcpClient client;
    private NetworkStream stream;
    public StreamWriter sw { get; set; }

    public event Action<string> MessageReceived;

    public async Task ConnectAsync(string serverIp, int serverPort)
    {
        client = new TcpClient();
        await client.ConnectAsync(serverIp, serverPort);
                 
        _=StartListening();
        
        
    }

    public async Task SendAsync(string message)
    {
       await sw.WriteAsync($"{message}\n");
        await sw.FlushAsync();
    }

    private async Task StartListening()
    {
        
        using var stream = client.GetStream();
        using var sr = new StreamReader(stream, Encoding.UTF8);
        sw=new StreamWriter(stream, Encoding.UTF8);
        

        while (true)
        {
            var msg = await sr.ReadLineAsync();
           
            MessageReceived?.Invoke(msg);

        }
        client?.Close();

    }
}
