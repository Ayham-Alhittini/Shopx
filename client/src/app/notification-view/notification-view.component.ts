import { Component, OnInit } from '@angular/core';
import { NotificationsService } from '../services/notifications/notifications.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationModel } from '../models/notification';

@Component({
  selector: 'app-notification-view',
  templateUrl: './notification-view.component.html',
  styleUrls: ['./notification-view.component.scss']
})
export class NotificationViewComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private notificationService: NotificationsService
  ) {}

  notificationId: number;
  notification: NotificationModel;
  
  ngOnInit(): void {
    this.notificationId = parseInt(this.route.snapshot.params['id']);

    this.notificationService.getNotifications().subscribe((res) => {
      this.notification = res.find((notification) => notification.id === this.notificationId);
      this.notificationService.readNotification(this.notification.id).subscribe();
    });
  }




}
