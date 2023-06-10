import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ShopRequest } from 'src/app/models/ShopRequest';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-shop-requests',
  templateUrl: './shop-requests.component.html',
  styleUrls: ['./shop-requests.component.scss']
})
export class ShopRequestsComponent implements OnInit{
  constructor(private adminService: AdminService){}

  requests : ShopRequest[]= [];

  ngOnInit(): void {
    this.adminService.getShopRequests().subscribe({
      next : res => {
        this.requests = res;
        // console.log(this.requests);
      }
    });

    this.adminService.onRequestAnswer.subscribe({
      next: res => {
        this.requests = this.requests.filter((item) => item !== res);
      }
    });
  }
}
