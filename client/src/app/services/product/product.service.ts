import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  
  constructor(
    private http: HttpClient
  ) { }
  
  private baseUrl = environment.apiBase;
  
  getProducts(pageNumber: number, pageSize: number)  {
    return this.http.get<Product[]>(this.baseUrl + "seller/get-my-product?pagenumber=" + pageNumber + "&pagesize=" + pageSize);
  }
  
  getProductById(id: number) {
    return this.http.get<Product>(this.baseUrl + "user/get-product/" + id);
  }
  
  editProduct(productUrl, model: any, id: number) {
    return this.http.put(this.baseUrl + "seller/edit-product-" + productUrl + '/' + id, model);
  }
  addProduct(productUrl, model) {
    return this.http.post<Product>(this.baseUrl + "seller/add-product-" + productUrl, model);
  }
  
  deleteProduct(id: number) {
    return this.http.delete<Product>(this.baseUrl + "seller/delete-product/" + id);
  }

  setDiscountRate(productId: number, discountRate: number) {
    return this.http.put(this.baseUrl + 'seller/discount/' + productId + '?NewDiscountRate=' + discountRate, null);
  }

}
