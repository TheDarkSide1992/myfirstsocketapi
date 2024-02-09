using System.ComponentModel.DataAnnotations;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToSignInDto : BaseDto
{
    [MinLength(2)] public string UserName { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToSignIn : BaseEventHandler<ClientWantsToSignInDto>
{
    public override Task Handle(ClientWantsToSignInDto dto, IWebSocketConnection socket)
    {
        StateService.WsConections[socket.ConnectionInfo.Id].UserName = dto.UserName;
        Console.WriteLine(dto.UserName + "signed in");

        socket.Send("you signed in");
        return Task.CompletedTask;
    }
}