using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    [MinLength(8)]
    public string broadCastRoomMessage { get; set; }
    public int roomId { get; set; }
}

[ValidateUsername]
[ValidateDataAnnotations]
public class ClientWantsToBroadcastToRoom(Reposetory repo)  : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    public override Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        var message = new ServerBroadcastMessageToRoom()
        {
            Message = dto.broadCastRoomMessage,
            UserName = StateService.WsConections[socket.ConnectionInfo.Id].UserName
        };

        repo.saveMessage(dto.roomId, dto.broadCastRoomMessage, StateService.WsConections[socket.ConnectionInfo.Id].UserName);
        
        StateService.BroadCastRoom(dto.roomId,JsonSerializer.Serialize(message));
        
        //TODO Save Message To DB
        
        return Task.CompletedTask;
    }
}

public class ServerBroadcastMessageToRoom : BaseDto{
    public string Message { get; set; }
    public string UserName { get; set; }
}