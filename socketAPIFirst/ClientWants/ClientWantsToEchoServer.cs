using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.AIFilter;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToEchoServerDTO:BaseDto
{
    //public string eventType { get; set; }
    [MinLength(2)]
    public string content { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToEchoServer(Reposetory repo) : BaseEventHandler<ClientWantsToEchoServerDTO>
{
    public override async Task Handle(ClientWantsToEchoServerDTO dto, IWebSocketConnection socket)
    {
        Console.WriteLine("someone echoed");
        AiToxicFilter filter = new AiToxicFilter();
        await filter.IsMessageToxic(dto.content, repo, StateService.WsConections[socket.ConnectionInfo.Id].UserName);
        
        var echo = new ServerEchosClient()
        {
            echoValue = "echo: " + dto.content
        };
        var messageToClient = JsonSerializer.Serialize(echo);

        socket.Send(messageToClient);
        
    }
}

public class ServerEchosClient : BaseDto
{
    [MinLength(2)]
    public String echoValue {get; set; }
}