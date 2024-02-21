using System.Reflection;
using System.Text.Json;
using Fleck;
using lib;
using Serilog;
using socketAPIFirst;
using socketAPIFirst.middleWare;

public static class StartUp
{
    public static void Main(String[] args)
    {
        var app = StatUp(args);
        app.Run();
    }

    public static WebApplication StatUp(String[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "\n{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}\n")
            .CreateLogger();

        var server = new WebSocketServer("ws://0.0.0.0:8181");
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
            dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());

        var clientEventHandler = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());
        builder.Services.AddSingleton<Reposetory>();

        var app = builder.Build();

        server.Start(ws =>
        {
            ws.OnOpen = () =>
            {
                StateService.AddConection(ws);

                var serverNote = new ServerNotefication();
                serverNote.eventType = "ServerNotefication";
                serverNote.content = "a new member entered the chat: " + StateService.WsConections.Count;

                var messageToClient = JsonSerializer.Serialize(serverNote);

                foreach (var webSocketConnection in StateService.WsConections)
                {
                    webSocketConnection.Value.Connection.Send(messageToClient);
                }

                Console.WriteLine("\n connection count is currently: " + StateService.WsConections.Count);
            };

            ws.OnClose = () =>
            {
                StateService.WsConections.Remove(ws.ConnectionInfo.Id);

                var serverNote = new ServerNotefication();
                serverNote.eventType = "ServerNotefication";
                serverNote.content = "a member left the chat";

                var messageToClient = JsonSerializer.Serialize(serverNote);

                foreach (var webSocketConnection in StateService.WsConections)
                {
                    webSocketConnection.Value.Connection.Send(messageToClient);
                }

                Console.WriteLine("\n connection count is currently: " + StateService.WsConections.Count);
            };


            ws.OnMessage = async message =>
            {
                try
                {
                    await app.InvokeClientEventHandler(clientEventHandler, ws, message);
                }
                catch (Exception e)
                {
                    e.Handle(ws, e.Message);
                }
            };
        });

        return app;
    }
}

/*
    ws.OnMessage = message =>
    {
        var m = JsonSerializer.Deserialize<BaseDTOMessage>(message);
        if (m.eventType.Equals("poke"))
        {
            int toPoke = Convert.ToInt32(m.content);

            if (toPoke-1 > wsConections.Count)
            {
                ws.Send("you cant poke: " + m.content);
            }
            else if (toPoke <= wsConections.Count)
            {
                var victim = wsConections[toPoke-1];
                victim.Send("you have been poked");
                ws.Send("poke Successful");
            } else {
                ws.Send("could not poke :-(");
            }


            Console.WriteLine("poke " + message);
        }

        if (m.eventType.Equals("string"))
        {
            foreach (var webSocketConnection in wsConections)
            {
                if (webSocketConnection.Equals(ws))
                {
                    webSocketConnection.Send("Hello you. thanks for writing: " + m.content);
                }
                else
                {
                    webSocketConnection.Send("Hello you. someone wrote: " + m.content);
                }
            }
        }
       //wsConections[5].Send("hey");
       //wsConections[2].Send("ye6");
    };
    */

public class ServerNotefication : BaseDto
{
    public string eventType { get; set; }
    public Object content { get; set; }
}