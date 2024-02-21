using System.Text.Json;
using Fleck;
using lib;

namespace socketAPIFirst;

public class ServerNoteficationDto : BaseEventHandler<ClientWantsToEchoServerDTO>
{
    public override Task Handle(ClientWantsToEchoServerDTO dto, IWebSocketConnection socket)
    {
        var note = new ServerNotefication()
        {
            //NoteValue = "a new member entered the chat" + Conections.wsConections.Select(c => c.ConnectionInfo.Id.ToString())
            NoteValue = "a new member entered " + StateService.WsConections.Count
        };
        var messageToClient = JsonSerializer.Serialize(note);

        foreach (var webSocketConnection in StateService.WsConections)
        {
            webSocketConnection.Value.Connection.Send(messageToClient);
        }
        
        Console.WriteLine("\n connection count is currently: " + StateService.WsConections.Count);

        return Task.CompletedTask;
    }
}

public class ServerNotefication : BaseDto
{
    public String NoteValue {get; set; }
}
