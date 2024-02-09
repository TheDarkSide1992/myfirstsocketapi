using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToEnterRoomDto : BaseDto
{
    public int roomId { get; set; }
    
}

[ValidateUsername]
[ValidateDataAnnotations]
public class ClientWantsToEnterRoom(Reposetory repo) : BaseEventHandler<ClientWantsToEnterRoomDto>
{
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        var joinedRoom = StateService.AddToRoom(socket,dto.roomId);
        socket.Send(JsonSerializer.Serialize(new ServerAddslientToRoom()
        {
            message = socket.ConnectionInfo.Id + " Conected to room " + dto.roomId
        }));

        var messagesArr = new ServerSendsOlderMessagesToClient()
        {
            messages = repo.getMessage(dto.roomId)
        };
        
        socket.Send(JsonSerializer.Serialize(messagesArr));
        
        return Task.CompletedTask;
    }
}

public class ServerAddslientToRoom : BaseDto
{
    [MinLength(2)]
    public string message { get; set; }
}

public class ServerSendsOlderMessagesToClient : BaseDto {
    public IEnumerable<ServerBroadcastMessageToRoom> messages { get; set; }
}