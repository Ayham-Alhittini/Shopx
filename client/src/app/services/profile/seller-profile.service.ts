import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SellerProfileService {

  constructor(
    private http : HttpClient
  ) { }

  private baseUrl = environment.apiBase + 'seller/';

  getSellerProfile() {
    return this.http.get(this.baseUrl + "my-profile");
  }

  editSellerProfile(model) {
    return this.http.put(this.baseUrl + "edit-account", model);
  }
}
