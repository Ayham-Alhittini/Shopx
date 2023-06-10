import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { CartService } from '../customer-product/cart.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http : HttpClient,
    private router: Router,
    private cartService: CartService
  ) { }

  private baseUrl = environment.apiBase;

  private _loadedUser$ = new BehaviorSubject<User>(null);
  get loadedUser$() {
    return this._loadedUser$.asObservable();
  }
  get loadedUser() {
    return this._loadedUser$.value;
  }

  login(model : any)
  {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      tap(
        res => {
          this.setCurrentUser(res);
        },
      )
    );
  }


  autoLogin()
  {
    const user = localStorage.getItem('user');
    
    if (user) {
      const userObj = JSON.parse(user);
      this._loadedUser$.next(userObj);
    }
  }

  logout() {
    localStorage.removeItem('user');
    this._loadedUser$.next(null);
    this.cartService.cartCount = 0;
    this.router.navigate(['/login']);
  }

  registerSeller(model : any) {
    return this.http.post<User>(this.baseUrl + "account/register-seller", model).pipe(
      tap(
        res => {
          this.setCurrentUser(res);
        },
      )
    );
  }

  registerCustomer(model : any) {
    return this.http.post<User>(this.baseUrl + "account/register-customer", model).pipe(
      tap(
        res => {
          this.setCurrentUser(res);
        },
      )
    );
  }

  setCurrentUser(user : User) {
    this._loadedUser$.next(user);
    localStorage.setItem('user', JSON.stringify(user));
  }

  resendConfirmation(email: string) {
    return this.http.post<string>(this.baseUrl + "account/resend-confirm-email/" + email, null);
  } 

  resetPassword(model: any) {
    return this.http.post(this.baseUrl + "account/reset-password", model);
  }

  sendResetEmail(email: string) {
    return this.http.post<string>(this.baseUrl + "account/forgot-password?email=" + email, null);
  }

  changePassword(model: any) {
    return this.http.post(this.baseUrl + "account/change-password", model);
  }

  chechUserExist(model) {
    const params = new HttpParams()
    .set('username', model['username'] + '')
    .set('email', model['email'] + '');
    return this.http.get<null>(this.baseUrl + "account/check-user-exist", {params});
  }
  

  checkShopName(shopname: string) {
    return this.http.get(this.baseUrl + "account/unique-shopname?shopname=" + shopname);
  }
}
