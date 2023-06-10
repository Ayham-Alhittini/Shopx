import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';
import { HandsetService } from '../services/handset/handset.service';
import { NotificationsService } from '../services/notifications/notifications.service';
import { NotificationModel } from '../models/notification';
import { User } from '../models/user';
import { CartService } from '../services/customer-product/cart.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  isHandset$: Observable<boolean> = this.handsetObserver.getIsHandset();

  displaySearchbar: boolean = false;
  notificationsArray: NotificationModel[] = [];

  user: User;
  isCustomer = false;
  
  constructor(
    private handsetObserver: HandsetService,
    private authService: AuthService,
    private notificationService: NotificationsService,
    public cartService: CartService
  ) {}

  ngOnInit(): void {

    this.user = this.authService.loadedUser;
    if (this.user !== null && this.user.accountType === 'Customer')
      this.isCustomer = true;
    
    if (this.isCustomer) {
      this.notificationService.getNotifications().subscribe((res) => {
        this.notificationsArray = res;
      });
    }
  }

  logout() {
    this.authService.logout();
  }

  toggleSearchbar() {
    this.displaySearchbar = this.displaySearchbar === false ? true : false;
  }

  isGuest() {
    return this.user === null;
  }

  isSeller() {
    return this.user !== null && this.user.accountType === 'Seller';
  }

  isAdmin() {
    return this.user !== null && this.user.accountType === 'Admin';
  }
}
