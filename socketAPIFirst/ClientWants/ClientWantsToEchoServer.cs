using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToEchoServerDTO:BaseDto
{
    //public string eventType { get; set; }
    [MinLength(2)]
    public string content { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToEchoServer : BaseEventHandler<ClientWantsToEchoServerDTO>
{
    public override Task Handle(ClientWantsToEchoServerDTO dto, IWebSocketConnection socket)
    {
        Console.WriteLine("someone echoed");
        
        var echo = new ServerEchosClient()
        {
            echoValue = "echo: " + dto.content
        };
        var messageToClient = JsonSerializer.Serialize(echo);

        socket.Send(messageToClient);
        
        
        return Task.CompletedTask;
    }
}

public class ServerEchosClient : BaseDto
{
    [MinLength(2)]
    public String echoValue {get; set; }
}