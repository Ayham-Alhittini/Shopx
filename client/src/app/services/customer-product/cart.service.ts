import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Product } from 'src/app/models/Product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private http: HttpClient) { }

  private baseUrl = environment.apiBase + 'customer/';

  cartCount = 0;

  addToCart(product: { productId: number, quantity: number }) {
    return this.http.post(this.baseUrl + 'add-to-cart/', product);
  }

  removeFromCart(productId: number) {
    return this.http.delete(this.baseUrl + 'delete-from-cart/' + productId);
  }

  getCart() {
    return this.http.get<{ product: Product; quantity: number }[]>(this.baseUrl + 'my-cart');
  }
}
