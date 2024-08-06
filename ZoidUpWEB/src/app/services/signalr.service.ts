import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  hubConnection!: HubConnection;
  MessageReceived = new Subject<any>();
  HasJoined = new Subject<any>();

  constructor(private http: HttpClient) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7224/chathub')
      .build();
  }

  StartConnection() {
    this.hubConnection
      .start()
      .then(() => {
        console.log('connection started!');
      })
      .catch((err) => {
        console.log(err);
      });

    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      this.MessageReceived.next({ user: user, message: message });
    });

    this.hubConnection.on('HasJoined', (message: string) => {
      this.HasJoined.next(message);
    });
  }

  SendMessageToAll(user: string, message: string) {
    this.hubConnection.invoke('SendMessageToAll', user, message);
  }
}
