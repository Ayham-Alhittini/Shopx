import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../validators/custom-validators';
import { AuthService } from '../services/auth/auth.service';
import { InitProductService } from '../services/init/init-product.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { MatStepper } from '@angular/material/stepper';

@Component({
  selector: 'app-seller-signup',
  templateUrl: './seller-signup.component.html',
  styleUrls: ['./seller-signup.component.scss']
})
export class SellerSignupComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private initService: InitProductService,
    private _snackbar: MatSnackBar,
    private router : Router
  ) {}




  signupForm: FormGroup;
  shopForm: FormGroup;
  signupDetailsForm: FormGroup;
  cityOptions = [];
  disableSubmit = false;

  displayConfirm = false;

  ngOnInit(): void {
    this.initService.getCities().subscribe(cities => this.cityOptions = cities);

    this.signupForm = this.fb.group({
      username: ['', { validators: [Validators.required] }],
      email: ['', { validators: [Validators.required, Validators.email] }],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]],
      confirmPassword: ['', { validators: [Validators.required] }]
    },
    {
      validators: [CustomValidators.passwordsMatch]
    });
    this.shopForm = this.fb.group({
      phoneNumber: ['', { validators: [Validators.required, Validators.minLength(9)] }],
      knownAs: ['', { validators: [Validators.required] }],
      shopCity: ['', { validators: [Validators.required] }],
      shopDescription: ['', { validators: [Validators.required] }],
    });
    this.signupDetailsForm = this.fb.group({
      taxNumber: ['', { validators: [Validators.required] }],
      fullName: ['', { validators: [Validators.required] }],
      bankName: ['', { validators: [Validators.required] }],
      bankAccountNumber: ['', { validators: [Validators.required] }],
    });
  }

  submit() {
    this.disableSubmit = true;

    var seller = {
      username: this.signupForm.get('username').value,
      email: this.signupForm.get('email').value,
      password: this.signupForm.get('password').value,
      phoneNumber: '+962' + this.shopForm.get('phoneNumber').value,
      knownAs: this.shopForm.get('knownAs').value,
      shopCity: this.shopForm.get('shopCity').value,
      shopDescription: this.shopForm.get('shopDescription').value,
      taxNumber: '' + this.signupDetailsForm.get('taxNumber').value,
      fullName: this.signupDetailsForm.get('fullName').value,
      bankName: this.signupDetailsForm.get('bankName').value,
      bankAccountNumber: '' + this.signupDetailsForm.get('bankAccountNumber').value,
    }


    this.auth.registerSeller(seller).subscribe({
      next: () => this.displayConfirm = true,
      error: (error) => {
        console.log(error)
        if(error && error.error && !error.error.message ) {
          this.openSnackBar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackBar("an error occured during signup, please try again", "ok");
        }

        this.disableSubmit = false;
      }
    })
  }

  goForward(stepper: MatStepper){
    
    if (this.signupForm.invalid)return;

    const model = {
      username : this.signupForm.get('username').value,
      email: this.signupForm.get('email').value
    }
    this.auth.chechUserExist(model).subscribe({
      next: _ => {
        stepper.next();
      },
      error: err => {
        this.openSnackBar(err.error, 'x');
      }
    });
  }
  goForward2(stepper: MatStepper) {

    if (this.shopForm.invalid)return;

    this.auth.checkShopName(this.shopForm.get('knownAs').value).subscribe({
      next: _ => stepper.next(),
      error: err => {
        this.openSnackBar(err.error, 'X');
      }
    })
  }

  openSnackBar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
}
