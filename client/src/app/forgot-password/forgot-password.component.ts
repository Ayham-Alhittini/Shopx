import { Component } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { delay, timer } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) { }

  confirmForm!: FormGroup;
  message = '';
  disable = false;

  ngOnInit(): void {
    this.confirmForm = this.fb.group({
      email: ['', { validators: [Validators.required, Validators.email] }]
    });
  }

  sendResetEmail() {
    
    this.authService.sendResetEmail(this.confirmForm.get('email').value).subscribe({
      next: () => {
        this.message = "an email has been sent to you";
        this.timeoutButton();
      }
    });
  }

  timeoutButton() {
    this.disable = true;
        setTimeout(() => {
          this.disable = false;
        }, 3000);
  }

}
