using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.AIFilter;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    [MinLength(2)]
    public string broadCastRoomMessage { get; set; }
    public int roomId { get; set; }
}

[ValidateUsername]
[ValidateDataAnnotations]
public class ClientWantsToBroadcastToRoom(Reposetory repo)  : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    public override async Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        AiToxicFilter filter = new AiToxicFilter();
        await filter.IsMessageToxic(dto.broadCastRoomMessage, repo, StateService.WsConections[socket.ConnectionInfo.Id].UserName);
        
        var message = new ServerBroadcastMessageToRoom()
        {
            Message = dto.broadCastRoomMessage,
            UserName = StateService.WsConections[socket.ConnectionInfo.Id].UserName
        };

        repo.saveMessage(dto.roomId, dto.broadCastRoomMessage, StateService.WsConections[socket.ConnectionInfo.Id].UserName);
        
        StateService.BroadCastRoom(dto.roomId,JsonSerializer.Serialize(message));
        
        //TODO Save Message To DB
        
    }
}

public class ServerBroadcastMessageToRoom : BaseDto{
    [MinLength(2)]
    public string Message { get; set; }
    [MinLength(2)]
    public string UserName { get; set; }
}