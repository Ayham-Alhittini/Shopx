import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SellerPhotosService {

  constructor(private http: HttpClient) { }

  private baseUrl = environment.apiBase + "seller/";

  uploadProductPhoto(id: number, file: FormData) {
    return this.http.post<{ url: string, id: number }>(this.baseUrl + "add-photo-to-product/" + id, file);
  }

  deleteProductPhoto(productId, photoId) {
    return this.http.delete(this.baseUrl + "delete-photo-from-product/" + productId + "/" + photoId);
  }
}
