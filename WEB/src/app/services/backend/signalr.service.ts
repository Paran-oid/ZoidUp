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
import { CreateMessageDto } from '../../models/main/message.model';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  public hubConnection!: HubConnection;
  private signalrSession = new Subject();
  public signalrSession$ = this.signalrSession.asObservable();
  private connection: Connection | null = null;

  constructor(
    private notificationService: NotificationService,
    private messageService: MessageService
  ) {}
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
      this.SendMessageListener();
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

  SendMessage(message: CreateMessageDto) {
    this.hubConnection
      .invoke('SendMessage', message)
      .then((message) => {
        this.messageService.messages.next(message);
      })
      .catch((err) => console.error(err));
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

  SendMessageListener() {
    this.hubConnection.on('SendMessageSuccess', (response) => {
      console.log(response);
    });
    this.hubConnection.on('SendMessageFailure', (response) => {
      console.log(response);
    });
  }

  ConnectionListener() {
    this.hubConnection.on('Disconnected', (response) => {});
    this.hubConnection.on('Connected', (response) => {});
  }
}
