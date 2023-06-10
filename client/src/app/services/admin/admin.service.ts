import { HttpClient, HttpParams } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { ShopRequest } from 'src/app/models/ShopRequest';
import { User } from 'src/app/models/user';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from '../paginationHelper';
import { Product } from 'src/app/models/Product';
import { Report } from 'src/app/models/report';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }
  
  private baseApi = environment.apiBase + 'admin/';

  onRequestAnswer = new EventEmitter<ShopRequest>();
  onChangePasswordDialogClosed = new EventEmitter<void>();

  getShopRequests() {
    return this.http.get<ShopRequest[]>(this.baseApi + 'get-shops-request');
  }

  rejectShop(userName: string, rejectReason: string) {
    return this.http.put(this.baseApi + 'reject-shops/' + userName + '?rejectionReason='+rejectReason,null);  
  }

  acceptShop(userName: string) {
    return this.http.put(this.baseApi + 'accept-shops/' + userName, null);  
  }

  getUsers(userType: string, userState: string, pageNumer: number, pageSize: number) {
    return getPaginatedResult<User[]>(this.baseApi + 'get-'+ userState +'-users?AccountType='+ userType
    , getPaginationHeaders(pageNumer, pageSize),
     this.http);
  }

  getUser(userInfo: string) {
    return this.http.get<User>(this.baseApi + 'get-user/' + userInfo);
  }

  blockUser(userInfo: string, command: boolean) {
    return this.http.put(this.baseApi + 'block-user/' + userInfo + '?blockCommand=' + command, null);
  }

  blockProduct(productId: number, blockReason: string) {
    return this.http.put(this.baseApi + 'block-product/' + productId + '?blockCommand=true&reason=' + blockReason
    , null);
  }

  unBlockProduct(productId: number) {
    return this.http.put(this.baseApi + 'block-product/' + productId + '?blockCommand=false', null);
  }

  getProducts(pageNumer: number, pageSize: number, state: string, sellerName: string) {
    return getPaginatedResult<Product[]>(this.baseApi + 'get-products?ProductState=' + state + '&SellerName=' + sellerName
    , getPaginationHeaders(pageNumer, pageSize), this.http);
  }

  getShopsSearch() {
    return this.http.get<[]>(environment.apiBase + 'user/get-shops');
  }

  getProduct(productId: number) {
    return this.http.get<Product>(this.baseApi + 'get-product/' + productId);
  }

  getReports(productId: number) {
    return this.http.get<Report[]>(this.baseApi + 'get-product-reports/' + productId);
  }

  watchReport(reportId: number) {
    return this.http.put(this.baseApi + 'watch/' + reportId, null);
  }
}
