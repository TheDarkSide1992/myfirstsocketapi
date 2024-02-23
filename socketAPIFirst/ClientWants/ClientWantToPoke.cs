﻿using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.middleWare;

namespace socketAPIFirst;


[ValidateDataAnnotations]
public class ClientWantToPoke : BaseEventHandler<ClientWantsToEchoServerDTO>
{
    //This feature needs to be updated to function together with the current stateservice in order to function
    
    public override Task Handle(ClientWantsToEchoServerDTO dto, IWebSocketConnection socket)
    {
        Console.WriteLine("someone poked: " +dto.content );
        
        int toPoke = Convert.ToInt32(dto.content);

        if (toPoke-1 > StateService.WsConections.Count)
        {
            var pokeClient = new ServerPokeClient()
            {
                pokeValue = "you cant poke: " + dto.content
            };
            var messageToClient = JsonSerializer.Serialize(pokeClient);
            socket.Send(messageToClient);
        }
        else if (toPoke <= StateService.WsConections.Count)
        {
            var messageVictim = new ServerPokeClient()
            {
                pokeValue = "you have been poked"
            };
            var messageToVictimClient = JsonSerializer.Serialize(messageVictim);
            var pokeClient = new ServerPokeClient()
            {
                pokeValue = "you have poked: " + dto.content
            };
            var messageToClient = JsonSerializer.Serialize(pokeClient);
            
            
           /* var victim = StateService.WsConections.Values.Count[toPoke-1];
            victim.Send(messageToVictimClient);
            socket.Send(messageToClient);*/
        } else {
            var pokeClient = new ServerPokeClient()
            {
                pokeValue ="could not poke :-("
            };
            var messageToClient = JsonSerializer.Serialize(pokeClient);
            socket.Send(messageToClient);
        }


        return Task.CompletedTask;
    }
}

public class ServerPokeClient : BaseDto
{
    public String pokeValue {get; set; }
}