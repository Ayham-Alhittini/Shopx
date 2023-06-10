import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BrowseHistoryService {

  private baseUrl = environment.apiBase + 'browsehistory/'

  constructor(private http: HttpClient) { }

  getHistory() {
    return this.http.get<Product[]>(this.baseUrl + 'my-browse-history');
  }

  removeFromHistory(productId) {
    return this.http.delete(this.baseUrl + 'delete-from-browse-history/' + productId);
  }

  deleteAll() {
    return this.http.delete(this.baseUrl + 'clear-browse-history');
  }

}
