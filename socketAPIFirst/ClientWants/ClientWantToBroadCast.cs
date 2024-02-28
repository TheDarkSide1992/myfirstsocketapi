using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.AIFilter;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;


[ValidateDataAnnotations]
public class ClientWantToBroadCast(Reposetory repo) : BaseEventHandler<ClientWantsToEchoServerDTO>
{
    public override async Task Handle(ClientWantsToEchoServerDTO dto, IWebSocketConnection socket)
    {
        AiToxicFilter filter = new AiToxicFilter();
        await filter.IsMessageToxic(dto.content, repo, StateService.WsConections[socket.ConnectionInfo.Id].UserName);
        
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

    }
}

public class ServerBroadcastClients : BaseDto
{
    [MinLength(2)]
    public String broadcastValue {get; set; }
}