import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from 'src/app/models/categories';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InitProductService {

  constructor(
    private http: HttpClient
  ) { }

  private baseUrl = environment.apiBase + 'initialization/';

  getCategories() {
    return this.http.get<Category[]>(this.baseUrl + "get-categories");
  }

  getModel(categoryLink: string) {
    return this.http.get(this.baseUrl + "get-" + categoryLink);
  }

  getCities() {
    return this.http.get<string[]>(this.baseUrl + 'get-cities');
  }
}
