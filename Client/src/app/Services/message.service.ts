import { HttpClient } from '@angular/common/http';
import { Message } from 'src/app/Models/Message';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../Models/User';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  url: string = environment.url + 'message/';

  constructor(private http: HttpClient) { }

  getContacts(userId: string): Observable<User[]> {
    return this.http.get<User[]>(this.url + 'contact/' + userId);
  }

  getMessages(userId: string, otherUser: string): Observable<Message[]> {
    return this.http.get<Message[]>(this.url + userId + '/' + otherUser);
  }

  postMessage(message: any):Observable<Message> {
    return this.http.post<Message>(this.url, message);
  }
}
