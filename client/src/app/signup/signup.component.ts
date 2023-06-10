import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { CustomValidators } from '../validators/custom-validators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private _snackbar: MatSnackBar,
    private router: Router
  ) { }

  firstSignupForm!: FormGroup;
  secondSignupForm!: FormGroup;
  displayConfirm: boolean = false;

  ngOnInit(): void {
    this.firstSignupForm = this.fb.group({
      name: ['', { validators: [Validators.required] }],
      phoneNumber: ['', { validators: [Validators.required, Validators.pattern(".{9,9}")] }],
    }
    );
    this.secondSignupForm = this.fb.group({
      email: ['', { validators: [Validators.required, Validators.email] }],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]],
      confirmPassword: ['', { validators: [Validators.required] }],
    },
    {
      validators: [CustomValidators.passwordsMatch]
    }
    );
  }

  disableSubmit = false;

  register() {
    this.disableSubmit = true;

    const user = {
      knownAs: this.firstSignupForm.get('name')?.value,
      phoneNumber: '962' + this.firstSignupForm.get('phoneNumber').value,
      email: this.secondSignupForm.get('email')?.value,
      password: this.secondSignupForm.get('password')?.value
    }
    this.authService.registerCustomer(user).subscribe({
      next: res => {
        this.displayConfirm = true;
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackBar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackBar("an error occured during signup, please try again", "ok");
        }

        this.disableSubmit = false;
      }
    });
  }

  openSnackBar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }

}
