import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  constructor(private http: HttpClient) {}
  public hubConnection!: HubConnection;
  StartConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chathub', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    this.hubConnection.start();
  }

  //Actions
  Authenticate(userId: number) {
    this.hubConnection.invoke('Authenticate', userId).then(() => {
      window.location.reload();
    });
  }

  //Listeners
  AuthenticateListener() {
    this.hubConnection.on('AuthSuccess', (response) => {
      console.log(response);
    });
    this.hubConnection.on('AuthFailed', (response) => {
      console.log(response);
    });
  }
}
