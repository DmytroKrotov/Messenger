using ServerMessenger.CommunicationItems;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class Server
{
    public Dictionary<string, string> passwordDictionary { get; set; }
    public Dictionary<string, ClientHandler> ConnectedUsers { get; set; }
    public List<ClientHandler> clients { get; set; }
    private TcpListener listener;
    
    public event Action<string, ClientHandler> MessageReceived;

    public Server(int port)
    {
        passwordDictionary= new Dictionary<string, string>() 
        {
            {"Dima","12345" },
            {"Ivan","123456" },
            {"Senya","123457" }
        };
        ConnectedUsers= new Dictionary<string, ClientHandler>();
        clients = new List<ClientHandler>();
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
    }

    public async Task StartAsync()
    {
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            
            ClientHandler clientHandler = new ClientHandler(client, this);
            
            

            Task.Run(() => clientHandler.HandleClientAsync());
        }
    }

    public async  Task BroadcastMessage(string message, ClientHandler sender)
    {
        
        foreach (ClientHandler client in ConnectedUsers.Values)
        {
            
            if (client != sender)
            {
                _=client.SendMessage(message);
            }
        }
    }
}

public class ClientHandler
{


    public string Name { get; set; }
    private TcpClient client;
    private NetworkStream stream;
    public StreamReader sr { get; set; }
    public StreamWriter sw { get; set; }
    private Server server;

    public ClientHandler(TcpClient client, Server server)
    {
        this.client = client;
        stream = client.GetStream();
        this.server = server;
        sr = new StreamReader(stream,Encoding.UTF8);
        sw=new StreamWriter(stream,Encoding.UTF8);
    }

    public async Task HandleClientAsync()
    {
        

        while (true)
        {
            var receivedMessage = await sr.ReadLineAsync();

            

            Console.WriteLine(receivedMessage);
            var parsedMessage=MessageHandler.ParseIncomeMessage(receivedMessage);

            var command=parsedMessage.Command;
            Console.WriteLine(command);
            switch (command)
            {
                case "LOGIN":
                    {
                        if (server.passwordDictionary.ContainsKey(parsedMessage.Sender)&& server.passwordDictionary[parsedMessage.Sender]==parsedMessage.Message)
                        {
                            
                            this.Name = parsedMessage.Sender;

                            if(!server.ConnectedUsers.ContainsKey(Name))
                            server.ConnectedUsers.Add(Name, this);
                            foreach (var item in server.ConnectedUsers)
                            {
                                if (item.Key != Name)
                                {
                                    _=SendMessage($"ADDUSER|SERVER|ALL|{item.Key}");
                                }
                                Console.WriteLine(item.Key+item.Value.ToString());
                            }


                            await server.BroadcastMessage($"ADDUSER|{Name}|ALL|{Name}",this);
                           
                            
                        }
                        break;
                    }
                case "REGISTER":
                    {
                        server.passwordDictionary.Add(parsedMessage.Sender, parsedMessage.Message);

                        if (server.passwordDictionary.ContainsKey(parsedMessage.Sender) && server.passwordDictionary[parsedMessage.Sender] == parsedMessage.Message)
                        {

                            this.Name = parsedMessage.Sender;

                            if (!server.ConnectedUsers.ContainsKey(Name))
                                server.ConnectedUsers.Add(Name, this);
                            foreach (var item in server.ConnectedUsers)
                            {
                                Console.WriteLine(item.Key + item.Value.ToString());
                            }


                            await server.BroadcastMessage($"ADDUSER|{Name}|ALL|{Name}", this);


                        }
                        break;
                    }
                   
                case "SEND":
                    {
                        var recipient = parsedMessage.Recipient;
                        var senger = parsedMessage.Sender;
                        var message = parsedMessage.Message;
                        await server.ConnectedUsers[recipient].SendMessage($"INCOME|{senger}|{recipient}|{message}");
                        break;
                    }
                   
                default:
                    
                    break;
            }
            //server.BroadcastMessage(receivedMessage, this);
            
        }
    }

    public async Task SendMessage(string message)
    {
        await sw.WriteAsync($"{message}\n");
        await sw.FlushAsync();
        Console.WriteLine("message sent");
    }
}
