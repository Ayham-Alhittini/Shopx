import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map, take } from 'rxjs';
import { AuthService } from 'src/app/services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AccountTypeGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const user = this.authService.loadedUser;
      
      if (user.accountType === route.data['accountType'] || user.accountType === 'Admin') {
        return true;
      }

      if (user.accountType === 'Seller') {
        this.router.navigateByUrl('/seller-profile')
      } else {
        this.router.navigateByUrl('/login');
      }
      return false;
  }
  
}
