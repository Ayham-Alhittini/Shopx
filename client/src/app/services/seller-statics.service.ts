import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class SellerStaticsService {

  constructor(private http: HttpClient) { }
  
  private baseApi = environment.apiBase + 'seller/';

  getCustomers(pageNumber: number, pagesize: number) {
    return getPaginatedResult<any>(this.baseApi + 'get-shop-customers'
    , getPaginationHeaders(pageNumber, pagesize), this.http);
  }

  getProductsStatics(pageNumber: number, pagesize: number) {
    return getPaginatedResult(this.baseApi + 'get-product-statics'
    , getPaginationHeaders(pageNumber, pagesize), this.http);
  }
  
}
