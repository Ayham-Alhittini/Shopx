import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(
    private http : HttpClient
  ) { }

  private baseUrl = environment.apiBase + 'user/';

  changeImage(file: FormData) {
    return this.http.put<{ id: number, url: string }>(this.baseUrl + "change-background", file);
  }

}
