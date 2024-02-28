using System.ComponentModel.DataAnnotations;
using Fleck;
using lib;
using socketAPIFirst.AIFilter;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;

public class ClientWantsToSignInDto : BaseDto
{
    [MinLength(2)] public string UserName { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToSignIn(Reposetory repo) : BaseEventHandler<ClientWantsToSignInDto>
{
    public override async Task Handle(ClientWantsToSignInDto dto, IWebSocketConnection socket)
    {
        if (repo.isUserBaned(dto.UserName))
        {
            throw new Exception("YOU ARE BANED");
        }
        
        AiToxicFilter filter = new AiToxicFilter();
        await filter.IsMessageToxic(dto.UserName,repo, StateService.WsConections[socket.ConnectionInfo.Id].UserName);
        
        StateService.WsConections[socket.ConnectionInfo.Id].UserName = dto.UserName;
        Console.WriteLine(dto.UserName + "signed in");

       // socket.Send("you signed in");
    }
}