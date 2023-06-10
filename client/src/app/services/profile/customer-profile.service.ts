import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerProfileService {

  constructor(
    private http : HttpClient
  ) { }

  private baseUrl = environment.apiBase + 'customer/';

  changePhoneNumber(phoneNumber) {
    return this.http.put(this.baseUrl + 'change-phone-number?phoneNumber=' + phoneNumber, '');
  }

  getPhoneNumber() {
    return this.http.get<{ phoneNumber: string }>(this.baseUrl + 'get-phone-number');
  }
}
