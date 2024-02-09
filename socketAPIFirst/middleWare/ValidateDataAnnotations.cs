using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using Fleck;
using lib;



namespace socketAPIFirst.middleWare;

public class ValidateDataAnnotations : BaseEventFilter
{
    public override Task Handle<T>(IWebSocketConnection socket, T dto)
    {
        var validationContext = new ValidationContext(
            dto ?? throw new ArgumentNullException(nameof(dto)));
        Validator.ValidateObject(dto, validationContext, true);
        return Task.CompletedTask;
    }
}

public class ValidateUsername : BaseEventFilter
{
    public override Task Handle<T>(IWebSocketConnection socket, T dto)
    {
        var u = StateService.WsConections[socket.ConnectionInfo.Id].UserName;
        if (string.IsNullOrEmpty(u))
            throw new AuthenticationException("You dont have a username");
        return Task.CompletedTask;
    }
}