import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { User } from '../models/user';
import {MatSnackBar} from '@angular/material/snack-bar';
import { GuestService } from '../services/guest.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private guestService: GuestService,
    private router: Router,
    private _snackBar: MatSnackBar 
  ) { }

  loginForm!: FormGroup;
  user!: User;

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', { validators: [Validators.required, Validators.email] }],
      password: ['', { validators: [Validators.required] }],
    });
  }

  login() {
    
    this.authService.login(this.loginForm.value).subscribe({
      next : (res) => {
        this.user = res;
        console.log(res);
        
        switch(res['accountType'])
        {
          case 'Admin': this.router.navigateByUrl("/admin");
            break;
          case 'Seller': this.router.navigateByUrl("/seller-profile");
            break;
          default: 
            this.router.navigateByUrl("/home");
            ///send guest data if any
            this.guestService.fillGuestCart().subscribe();
        }
      },
      error : error => {
        if(error && error.error && !error.error.message ) {
          this.openSnackBar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackBar("an error occured during login, please try again", "ok");
        }
  
        this.loginForm.reset(); /// keep or not
      }
    });
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, { duration: 3000 });
  }
}
