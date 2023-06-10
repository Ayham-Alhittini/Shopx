import { EventEmitter, Injectable } from "@angular/core";
import { Product } from "../models/Product";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject } from "rxjs";
import { environment } from "src/environments/environment";
import { CartService } from "./customer-product/cart.service";

@Injectable({
  providedIn: 'root'
})
export class GuestService {

    constructor(private http: HttpClient, private cartService: CartService){}

    guestCarts: { product: Product; quantity: number }[] = [];

    cartModified = new EventEmitter<void>();

    addToCart(carItem: { product: Product; quantity: number }){
      this.guestCarts.push(carItem);
      this.cartService.cartCount++;
      localStorage.setItem('guestCarts', JSON.stringify(this.guestCarts));
    }

    removeFromCart(productId: number) {
      const toDeleteCart = this.guestCarts.find(c => c.product.id === productId);
      this.guestCarts = this.guestCarts.filter(c => c !== toDeleteCart);

      this.cartService.cartCount -= toDeleteCart.quantity;
      localStorage.setItem('guestCarts', JSON.stringify(this.guestCarts));
      this.cartModified.emit();
    }

    onCart(productId: number) : boolean{
      return this.guestCarts.find(c => c.product.id === productId) != null;
    }

    getCartsLocal() {
      const carts = localStorage.getItem('guestCarts');
    
      
      if (carts) {
        this.guestCarts = JSON.parse(carts);
        var _cartCounter = 0;
        this.guestCarts.forEach(element => {
          _cartCounter += element.quantity;
        });

        this.cartService.cartCount = _cartCounter;
      }
    }

    increaseQuantity(product: { product: Product; quantity: number }) {
      const index = this.guestCarts.indexOf(product);
      this.guestCarts[index].quantity++;
      this.cartService.cartCount++;
      localStorage.setItem('guestCarts', JSON.stringify(this.guestCarts));
      this.cartModified.emit();
    }

    reduceQuantity(product: { product: Product; quantity: number }) {
      const index = this.guestCarts.indexOf(product);

      if (this.guestCarts[index].quantity === 1) {
        this.guestCarts.splice(index, 1);
      } else {
        this.guestCarts[index].quantity--;
      }

      this.cartService.cartCount--;
      localStorage.setItem('guestCarts', JSON.stringify(this.guestCarts));
      this.cartModified.emit();
    }

    fillGuestCart() {
        var carts : {productId: number, quantity: number}[] = [];
        this.guestCarts.forEach(element => {
          carts.push({productId: element.product.id, quantity: element.quantity});
        });
        this.guestCarts = [];
        localStorage.removeItem('guestCarts');

        ///add the carts on database to the nav
        this.cartService.getCart().subscribe({
          next: res => {
            res.forEach(element => {
              this.cartService.cartCount += element.quantity;
            });
          }
        })


        return this.http.post(environment.apiBase + 'customer/' + 'fill-guest-cart', carts);
    }
}
