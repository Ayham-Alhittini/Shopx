import { Component, ElementRef, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AdminService } from 'src/app/services/admin/admin.service';
import { AuthService } from 'src/app/services/auth/auth.service';
import { CustomValidators } from 'src/app/validators/custom-validators';

@Component({
  selector: 'app-change-password-dialog',
  templateUrl: './change-password-dialog.component.html',
  styleUrls: ['./change-password-dialog.component.scss']
})
export class ChangePasswordDialogComponent {

  showPassword = false;

  changePasswordForm: FormGroup;
  
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private adminService: AdminService,
    private toaster: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.changePasswordForm = this.fb.group({
      oldPassword: ['', { validators: [Validators.required] }],
      password: ['', { validators: [Validators.required] }],
      confirmPassword: ['', { validators: [Validators.required]}]
    },
    {
      validators: [CustomValidators.passwordsMatch]
    });
  }

  changePassword() {

    this.adminService.onChangePasswordDialogClosed.emit();
    const model = {
      oldPassword: this.changePasswordForm.get('oldPassword').value,
      newPassword: this.changePasswordForm.get('password').value
    }
    this.authService.changePassword(model).subscribe({
      next: () => this.toaster.open("password was changed succesfully", 'ok', { duration: 3000 }),
      error: err => {
        this.toaster.open(err.error, 'x', {
          duration: 3000
        });
      }
    });
  }


  onShowPassword() {
    this.showPassword = !this.showPassword;
  }
}
