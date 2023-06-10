import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomValidators } from '../validators/custom-validators';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent {

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  resetPasswordForm!: FormGroup;

  token : string;
  email: string;

  ngOnInit(): void {
    this.route.queryParamMap.subscribe( (params) => {
      this.email = params.get('email');
      this.token = params.get('token');
      
    });

    this.resetPasswordForm = this.fb.group({
      email: [this.email, { validators: [Validators.required, Validators.email] }],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]],
      confirmPassword: ['', { validators: [Validators.required, CustomValidators.passwordsMatch]}]
    });
  }

  resetPassword() {

    const model = {
      email: this.email,
      password: this.resetPasswordForm.get('password').value,
      token: this.token
    }

    this.authService.resetPassword(model).subscribe({
      next: () => this.router.navigate(['/login'])
    });

  }
}
