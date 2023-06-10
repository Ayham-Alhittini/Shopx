import { Component, OnInit } from '@angular/core';
import { Order } from '../models/order';
import { OrderService } from '../services/order/order.service';
import { HandsetService } from '../services/handset/handset.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  
  ordersArray: Order[] = [];
  columnsToDisplay = ['orderId', 'orderStatus', 'dateAdded', 'noOfProducts', 'totalPrice', 'view'];
  loading = true;

  constructor(
    private orderService: OrderService,
    private handsetService: HandsetService
  ) {}
  
  ngOnInit(): void {
    this.orderService.getOrders().subscribe({
      next: (orders) => {
        this.ordersArray = orders;
        this.loading = !this.loading;
      }
    })
  }

  



}
