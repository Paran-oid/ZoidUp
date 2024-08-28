import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { NotificationService } from '../frontend/notification.service';
import { ActivatedRoute } from '@angular/router';
import { Connection } from '../../models/main/connection.model';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  public hubConnection!: HubConnection;
  private signalrSession = new Subject();
  public signalrSession$ = this.signalrSession.asObservable();
  private connection: Connection | null = null;

  constructor(private notificationService: NotificationService) {}
  StartConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/chathub', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    this.hubConnection.start().then(() => {
      this.signalrSession.next({ type: HubConnectionState.Connected });

      //listeners
      this.ReauthenticateListener();
      this.AuthenticateListener();
      this.ConnectionListener();
    });
  }

  //Actions
  Authenticate(userId: number) {
    this.hubConnection.invoke('Authenticate', userId).then(() => {
      window.location.reload();
    });
  }

  Reauthenticate(userId: number) {
    this.hubConnection.invoke('Reauthenticate', userId).then(() => {
      //fix this problem
      // this.notificationService.Info('Loging in attempt...');
    });
  }

  //Listeners

  //used for logging in
  AuthenticateListener() {
    this.hubConnection.on('AuthSuccess', (response) => {});
    this.hubConnection.on('AuthFailed', (response) => {});
  }

  ReauthenticateListener() {
    this.hubConnection.on('ReauthSuccess', (connection: Connection) => {
      this.connection = connection;
      //fix this problem
      this.notificationService.Success('Reauthenticated!');
    });
    this.hubConnection.on('ReauthFailed', (response) => {});
  }

  ConnectionListener() {
    this.hubConnection.on('Disconnected', (response) => {});
    this.hubConnection.on('Connected', (response) => {});
  }
}
