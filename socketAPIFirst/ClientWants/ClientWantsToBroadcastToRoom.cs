using System.Text.Json;
using Fleck;
using lib;

namespace socketAPIFirst;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    public string broadCastRoomMessage { get; set; }
    public int roomId { get; set; }
}

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