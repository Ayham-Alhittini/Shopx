import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password-email',
  templateUrl: './reset-password-email.component.html',
  styleUrls: ['./reset-password-email.component.scss']
})
export class ResetPasswordEmailComponent {

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) { }

  confirmForm!: FormGroup;
  errMessage = false;

  ngOnInit(): void {
    this.confirmForm = this.fb.group({
      email: ['', { validators: [Validators.required, Validators.email] }]
    });
  }

  sendResetEmail() {
    this.authService.sendResetEmail(this.confirmForm.get('email').value).subscribe();
  }
}
