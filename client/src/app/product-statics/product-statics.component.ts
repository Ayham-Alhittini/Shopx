import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { Pagination } from '../models/pagination';
import { SellerStaticsService } from '../services/seller-statics.service';

@Component({
  selector: 'app-product-statics',
  templateUrl: './product-statics.component.html',
  styleUrls: ['./product-statics.component.scss']
})
export class ProductStaticsComponent implements OnInit {
  
  baseUrl = environment.apiBase + 'seller/';

  selectedOption: string = '1'; /// 1 mean show customers, 2 means show products statics

  pageNumber = 1;
  pageSize = 10;
  pagination: Pagination;
  constructor(private http: HttpClient, private sellerStaticsService: SellerStaticsService){}
  fullTotal = 0;
  staticsData = [];
  customers = [];
  displayOptions: string[] = 
  [
    'productName',
    'id',
    'state',
    'numberOfCustomers',
    'price',
    'discountRate',
    'onStock',
    'solidQuantity',
    'views',
    'total'
  ];

  displayOptions2 : string[] =
  [
    'backgroundPhoto',
    'id',
    'knownAs',
    'email',
    'isOnline',
    'created',
  ];

  ngOnInit(): void {
    this.loadStatics();
  }


  onChoiceChanged(event: any) {
    this.selectedOption = event.value;
    
    if (this.selectedOption === '1') {
      this.loadStatics();
    } else {
      this.loadCustomers();
    }
  }

  loadStatics() {
    this.sellerStaticsService.getProductsStatics(this.pageNumber, this.pageSize).subscribe({
      next: res => {
        this.fullTotal = res.result['total'];
        this.staticsData = res.result['statics'];
        this.pagination = res['pagination'];
      }
    });
  }

  loadCustomers() {
    this.sellerStaticsService.getCustomers(this.pageNumber, this.pageSize).subscribe({
      next: res => {
        this.customers = res['result'];
        console.log(this.customers);
        
        this.pagination = res['pagination'];
      }
    });
  }

  handlePageEvent(event: PageEvent) {

    // this.pageNumer = event.pageIndex + 1;
    // this.pageSize = event.pageSize;
    
    // this.getUsers();

    // //scroll to the top if page is changed.
    // document.querySelector('.mat-sidenav-content').scrollTop = 0;
  }

}
