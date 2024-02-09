using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class MessageDTO:BaseDto
{
    //public string eventType { get; set; }
    public string content { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToEchoServer : BaseEventHandler<MessageDTO>
{
    public override Task Handle(MessageDTO dto, IWebSocketConnection socket)
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
    public String echoValue {get; set; }
}