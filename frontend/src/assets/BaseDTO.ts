export class BaseDTO<T>{
  eventType: string;

  constructor(init?: Partial<T>) {
    this.eventType = this.constructor.name;
    Object.assign(this, init);
  }
}


export class ClientWantsToEchoServerDto extends BaseDTO<ClientWantsToEchoServerDto>{
  echoValue?: string;
}

export class ClientWantToBroadCast extends BaseDTO<ClientWantToBroadCast>{
  broadcastValue?: string;
}

export class ClientWantToPoke extends BaseDTO<ClientWantToPoke>{
  pokeValue?: string;
}

export class ServerNoteficationDto extends BaseDTO<ServerNoteficationDto>{
  content?: string;
}


