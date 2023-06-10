import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-resend-confirm',
  templateUrl: './resend-confirm.component.html',
  styleUrls: ['./resend-confirm.component.scss']
})
export class ResendConfirmComponent {

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private _snackbar: MatSnackBar
  ) { }

  confirmForm!: FormGroup;
  errMessage = false;

  ngOnInit(): void {
    this.confirmForm = this.fb.group({
      email: ['', { validators: [Validators.required, Validators.email] }]
    });
  }

  resend() {
    
    this.authService.resendConfirmation(this.confirmForm.get('email').value).subscribe({
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("an error occured, please try again", "ok");
        }
      } 
    });
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
}
