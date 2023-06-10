import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order } from 'src/app/models/order';
import { orderDetail } from 'src/app/models/order-detail';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }

  private baseUrl = environment.apiBase + 'customer/';

  checkout(model) {
    return this.http.post(this.baseUrl + 'checkout', model);
  }

  getOrders() {
    return this.http.get<Order[]>(this.baseUrl + 'my-orders');
  }

  getOrderDetails(id: number) {
    return this.http.get<orderDetail>(this.baseUrl + 'get-order-details/' + id);
  }
}
