using Fleck;

namespace socketAPIFirst;

public class WebSocketMetaData(IWebSocketConnection connection)
{
    public IWebSocketConnection Connection { get; set; } = connection;
    public string UserName { get; set; }
}

public static class StateService
{
    //public static List<IWebSocketConnection> wsConections { get; set; }
    public static Dictionary<Guid, WebSocketMetaData> WsConections = new();
    public static Dictionary<int, HashSet<Guid>> Rooms = new();

    public static bool AddConection(IWebSocketConnection ws)
    {
        return WsConections.TryAdd(ws.ConnectionInfo.Id, new WebSocketMetaData(ws));
    }

    public static bool AddToRoom(IWebSocketConnection ws, int roomId)
    {
        if(!Rooms.ContainsKey(roomId)) Rooms.Add(roomId, new HashSet<Guid>());
        return Rooms[roomId].Add(ws.ConnectionInfo.Id);
    }

    public static void BroadCastRoom(int room, string message)
    {
        var doesRoomExist = Rooms.TryGetValue(room, out var guids);
        if (!doesRoomExist)return;

        foreach (var guid in guids)
        {
            if (WsConections.TryGetValue(guid, out var ws)) ws.Connection.Send(message);
        }
    }
}