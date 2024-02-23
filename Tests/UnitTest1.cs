using lib;
using Newtonsoft.Json;
using socketAPIFirst;
using Websocket.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
       StartUp.StatUp(null);
    }
    
    
    [Test]
    public async Task BigBangTest()
    {
        var ws = await new WebSocketTestClient().ConnectAsync();
        var ws2 = await new WebSocketTestClient().ConnectAsync();

        await ws.DoAndAssert(new ClientWantsToSignInDto()
        {
            UserName = "Sarah"
        });
        await ws2.DoAndAssert(new  ClientWantsToSignInDto()
        {
            UserName =  "Jones"
        });
        
        
        await ws.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 1
        }, r => r.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1);
        await ws2.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 1
        }, r => r.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1);
        await ws.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 2
        }, r => r.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1);

        
        await ws.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
        {
            broadCastRoomMessage = "Hello my friend",
            roomId = 1
        }, m => m.Count(dto => dto.eventType == nameof(ServerBroadcastMessageToRoom)) == 1);
        await ws.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
        {
            broadCastRoomMessage = "Hello my friend I dont have",
            roomId = 2
        }, m => m.Count(dto => dto.eventType == nameof(ServerBroadcastMessageToRoom)) == 1);
        
        await ws2.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
        {
            broadCastRoomMessage = "Hello sarah",
            roomId = 1
        }, m => m.Count(dto => dto.eventType == nameof(ServerBroadcastMessageToRoom)) == 2);
        
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
    
    [Test]
    public void smalTest()
    {
        var wsClient = new WebsocketClient(new Uri("ws://localhost:8181"));
        wsClient.MessageReceived.Subscribe(msg =>
        {
            Console.WriteLine("MESSAGE FROM SERVER:  " + msg.Text);
        });
        wsClient.Start();

        var message = new ClientWantsToEchoServerDTO()
        {
            content = "Hello There"
        };
        
        wsClient.Send(JsonSerializer.Serialize(message));
        Task.Delay(5000).Wait();
            
        Assert.Pass();
    }

    [Test]
    public async Task miniTest()
    {
        var ws = await new WebSocketTestClient().ConnectAsync();

        await ws.DoAndAssert(new ClientWantsToEchoServerDTO()
        {
            content = "Holla Amiga"
        }, fromServer =>
        {
            return fromServer.Count(dto => dto.eventType == nameof(ServerEchosClient)) == 1;
        });
    }
}