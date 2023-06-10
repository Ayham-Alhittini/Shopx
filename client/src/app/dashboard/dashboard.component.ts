import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HandsetService } from '../services/handset/handset.service';
import { AuthService } from '../services/auth/auth.service';
import { User } from '../models/user';
import { MatDialog } from '@angular/material/dialog';
import { ChangePasswordDialogComponent } from '../admin/change-password-dialog/change-password-dialog.component';
import { AdminService } from '../services/admin/admin.service';
import { NotificationModel } from '../models/notification';
import { NotificationsService } from '../services/notifications/notifications.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit{

  notificationsArray: NotificationModel[] = [];
  constructor(
    private authService: AuthService,
    private adminService: AdminService,
    private handsetObserver : HandsetService,
    private dialog: MatDialog,
    private notificationService: NotificationsService
  ) {}
  user: User;
  isHandset$: Observable<boolean> = this.handsetObserver.getIsHandset();
  
  ngOnInit(): void {
    this.user = this.authService.loadedUser;

    this.adminService.onChangePasswordDialogClosed.subscribe({
      next: _ => {
        this.dialog.closeAll();
      }
    });

    this.notificationService.getNotifications().subscribe((res) => {
      this.notificationsArray = res;
    });
    
  }

  logout() {
    this.authService.logout();
  }
 
  changePassword() {
    this.dialog.open(ChangePasswordDialogComponent);
  }

}
