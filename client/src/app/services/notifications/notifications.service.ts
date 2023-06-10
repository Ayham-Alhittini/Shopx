import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NotificationModel } from 'src/app/models/notification';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  private baseUrl = environment.apiBase + 'notification/'

  constructor(private http: HttpClient) { }
  
  getNotifications() {
    return this.http.get<NotificationModel[]>(this.baseUrl + 'get-notifications');
  }

  readNotification(id: number) {
    return this.http.post(this.baseUrl + 'read-notification/' + id, '');
  }

}
