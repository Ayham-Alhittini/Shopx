import { Component } from '@angular/core';
import { AuthService } from './services/auth/auth.service';
import { GuestService } from './services/guest.service';
import { CartService } from './services/customer-product/cart.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'shopx';

  constructor(private authService : AuthService,
     private guestService: GuestService,
     private cartService: CartService){}

  ngOnInit(): void {
    this.autoLogin();

    if (this.authService.loadedUser === null)
      this.getGuestData();
    else if (this.authService.loadedUser.accountType === 'Customer') {
      
      this.cartService.getCart().subscribe({
        next: res => {
          res.forEach(element => {
            this.cartService.cartCount += element.quantity;
          });
        }
      });
    }
  }
  
  autoLogin()
  {
    this.authService.autoLogin();
  }
  
  getGuestData() {
    this.guestService.getCartsLocal();
  }

}
