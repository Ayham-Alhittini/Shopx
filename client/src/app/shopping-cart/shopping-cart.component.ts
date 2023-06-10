import { Component, OnInit } from '@angular/core';
import { CartService } from '../services/customer-product/cart.service';
import { Product } from '../models/Product';
import { BreakpointObserver } from '@angular/cdk/layout';
import { map } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';
import { GuestService } from '../services/guest.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.scss']
})
export class ShoppingCartComponent implements OnInit {

  handsetObserver;
  
  constructor(
    private cartService: CartService,
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
    private guestService: GuestService
  ) {
    this.handsetObserver = breakpointObserver
      .observe('(max-width: 599px)')
      .pipe(map(({matches}) => (matches ? true : false)));
  }
  
  products: { product: Product, quantity: number }[] = [];
  loading: boolean = true;
  totalPrice: number = 0;

  ngOnInit(): void {
    if (this.authService.loadedUser) {
      this.loadCustomer();
    } else {
      this.loading = false;
      this.loadCarts();
      this.guestService.cartModified.subscribe({
        next: _ => {
          this.loadCarts();
        }
      });
    }
  }

  removeFromCart(id: number) {
    this.cartService.removeFromCart(id).subscribe((res) => {

      this.loadCustomer();
      this.cartService.cartCount -= res['deletedQuantity'];
      
      ///// remove from dom.
      // this.products = this.products.filter(product => {
      //   return product.product.id !== id;
      // });
    });
  }

  updateTotalPrice(price: number) {
    this.totalPrice += price;
  }

  loadCarts() {
    if (this.authService.loadedUser === null)
      this.loadGuest();
    else 
      this.loadCustomer();
  }

  loadGuest() {
    this.products =  this.guestService.guestCarts;
    this.totalPrice = 0;
    for(let i = 0; i < this.products.length; i++) {
      this.totalPrice += this.products[i].product.price * this.products[i].quantity;
    }
  }

  loadCustomer() {
    this.totalPrice = 0;
    this.cartService.getCart().subscribe((products) => {
      this.products = products;
      this.loading = false;

      for(let i = 0; i < products.length; i++) {
        this.totalPrice += products[i].product.price * products[i].quantity;
      }
    });
  }


}
