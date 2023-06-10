import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/models/Product';
import { orderDetail } from 'src/app/models/order-detail';
import { OrderService } from 'src/app/services/order/order.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {

  details: orderDetail;
  salesArray: {
    id: number,
    customerId: string,
    customerUsername: string,
    sellerId: string,
    sellerName: string,
    productId: number,
    productName: string,
    price: number,
    quantity: number,
    product: Product,
    total: number,
    orderId: number
  }[] = [];

  columnsToDisplay = ['productName', 'quantity', 'productPrice', 'totalPrice'];

  loading = true;
  
  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap) => {
      const id = paramMap['params']['id'];

      this.orderService.getOrderDetails(id).subscribe({
        next: (details) => {
          this.details = details;
          this.salesArray = details.sales;
          this.loading = false;
        },
        error: () => this.loading = false
      });
    });
  }
}
