using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;


[ValidateDataAnnotations]
public class ClientWantToBroadCast : BaseEventHandler<MessageDTO>
{
    public override Task Handle(MessageDTO dto, IWebSocketConnection socket)
    {
        Console.WriteLine("someone broad cast");
        
        var broadcast = new ServerBroadcastClients()
        {
            broadcastValue = "BroadCast: " + dto.content
        };
        var messageToClient = JsonSerializer.Serialize(broadcast);

        foreach (var webSocketConnection in StateService.WsConections.Values)
        {
            webSocketConnection.Connection.Send(messageToClient);
        }

        return Task.CompletedTask;
    }
}

public class ServerBroadcastClients : BaseDto
{
    [MinLength(2)]
    public String broadcastValue {get; set; }
}