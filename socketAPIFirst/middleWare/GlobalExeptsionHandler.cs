using System.Text.Json;
using Fleck;
using lib;
using Serilog;

namespace socketAPIFirst.middleWare;

public static class GlobalExeptsionHandler
{
    public static void Handle(this Exception exception, IWebSocketConnection socket, string? message)
    {
        Log.Error(exception, "this Wass caught in gloabal handler");
        socket.Send(JsonSerializer.Serialize(new ServerSendsErrorMessageToClient()
        {
            recivedMessage = message,
            errorMessage = exception.Message
        }));

        if (exception.Message == "Dont speak like that" || exception.Message == "YOU ARE BANED")
        {
            //TODO Implement ban
            
            socket.Send(JsonSerializer.Serialize(new ServerSendsErrorMessageToClient()
            {
                recivedMessage =  StateService.WsConections[socket.ConnectionInfo.Id].UserName + " You are now baned",
                errorMessage = exception.Message
            }));
            
            socket.Close();
        }
    }
}

public class ServerSendsErrorMessageToClient : BaseDto
{
    public string recivedMessage { get; set; }
    public string errorMessage { get; set; }
}