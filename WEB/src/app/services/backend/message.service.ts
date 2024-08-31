import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { User } from '../../models/main/user.model';
import {
  Message,
  CreateMessageDto,
  EditMessageDto,
} from '../../models/main/message.model';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map } from 'rxjs';
import { Subject } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  url: string = '/api/messages';
  messages = new BehaviorSubject<Message[] | null>(null);
  messages$ = this.messages.asObservable();

  editMessages = new BehaviorSubject<number | null>(null);
  editMessages$ = this.editMessages.asObservable();

  removedMessages = new BehaviorSubject<number | null>(null);
  removedMessages$ = this.removedMessages.asObservable();

  constructor(private http: HttpClient) {}

  SendMessage(model: CreateMessageDto) {
    return this.http.post<Message>(this.url, model);
  }
  LoadMessages(userId: number, friendId: number) {
    return this.http
      .get<Message[]>(this.url + `/users/${userId}/${friendId}`)
      .pipe(
        map((messages) => {
          this.messages.next(messages);
        })
      );
  }
  Get(messageId: number) {
    return this.http.get<Message>(this.url + `${messageId}`);
  }
  Put(model: EditMessageDto) {
    return this.http.put<Message>(this.url, model);
  }
  Delete(messageId: number) {
    return this.http.delete(this.url + `/${messageId}`, {
      responseType: 'text',
    });
  }
}
