import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { map, take } from 'rxjs';
import { AuthService } from 'src/app/services/auth/auth.service';
import { CustomValidators } from 'src/app/validators/custom-validators';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {

  @Output() closeEvent = new EventEmitter();

  changePasswordForm: FormGroup;
  
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private _snackbar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.changePasswordForm = this.fb.group({
      oldPassword: ['', { validators: [Validators.required, Validators.email] }],
      password: ['', { validators: [Validators.required] }],
      confirmPassword: ['', { validators: [Validators.required]}]
    },
    {
      validators: [CustomValidators.passwordsMatch]
    });
  }

  changePassword() {
    
    const model = {
      oldPassword: this.changePasswordForm.get('oldPassword').value,
      newPassword: this.changePasswordForm.get('password').value,
    }

    this.authService.changePassword(model).subscribe({
      next: () => this.openSnackbar("password was changed succesfully", "ok")
    });

    this.closeForm();
  }

  closeForm() {
    this.closeEvent.emit();
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
}
