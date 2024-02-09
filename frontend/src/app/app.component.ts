import { Component } from '@angular/core';
import {FormControl, isFormControl} from '@angular/forms';
import {ReactiveFormsModule} from "@angular/forms";
import { RouterOutlet } from '@angular/router';
import {JsonPipe} from "@angular/common";
import {BaseDTO, ClientWantsToEchoServerDto, ClientWantToBroadCast, ClientWantToPoke, ServerNoteficationDto} from "../assets/BaseDTO";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ReactiveFormsModule, JsonPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontend';

  messages: string[] = [];

  ws: WebSocket = new WebSocket("ws://localhost:8181")
  messageContent = new FormControl('');


  constructor() {
    this.ws.onmessage = message => {
      const messageFromServer = JSON.parse(message.data) as BaseDTO<any>;
      //this.messages.push(message.data)

      //@ts-ignore
      this[messageFromServer.eventType].call(this, messageFromServer);
    }
  }

  ServerEchosClient(dto: ClientWantsToEchoServerDto){
    this.messages.push(dto.echoValue!);
  }

  ServerBroadcastClients(dto: ClientWantToBroadCast){
    this.messages.push(dto.broadcastValue!);
  }

  ServerPokeClient(dto: ClientWantToPoke){
    this.messages.push(dto.pokeValue!);
  }

  ServerNotefication(dto:ServerNoteficationDto){
    this.messages.push(dto.content!);
  }

  echoMessage() {
    var object = {
      eventType: "ClientWantsToEchoServer",
      content: this.messageContent.value!
    }

    this.ws.send(JSON.stringify(object));
  }

  SendPoke() {
    let object = {
      eventType: "ClientWantToPoke",
      content: this.messageContent.value!,
    }
    this.ws.send(JSON.stringify(object));
  }

  sendMessage() {
    var object = {
      eventType: "ClientWantToBroadCast",
      content: this.messageContent.value!
    }

    this.ws.send(JSON.stringify(object));
  }
}

/*
export interface BaseDTOMessage {
  eventType: string;
  content: string;
}*/
