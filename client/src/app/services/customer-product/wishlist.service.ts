import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {

  constructor(private http: HttpClient) { }

  private baseUrl = environment.apiBase + 'wishlist/';

  addToWishlist(productId: number) {
    return this.http.post(this.baseUrl + 'add-to-wishlist/' + productId, '');
  }

  removeFromWishlist(productId: number) {
    return this.http.delete(this.baseUrl + 'delete-from-wishlist/' + productId);
  }

  getWishlist(pageNumber='', pageSize='') {
    let query = '';
    if(pageNumber) {
      query += 'pageNumber=' + pageNumber + '&';
    }
    if(pageSize) {
      query += 'pageSize=' + pageSize + '&';
    }
    return this.http.get<Product[]>(this.baseUrl + 'my-wishlist?');
  }
}
