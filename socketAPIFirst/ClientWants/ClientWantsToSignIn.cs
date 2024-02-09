using Fleck;
using lib;

namespace socketAPIFirst;

public class ClientWantsToSignInDto : BaseDto
{
    public string UserName { get; set; }
}

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