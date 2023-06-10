import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomerProfileService } from 'src/app/services/profile/customer-profile.service';

@Component({
  selector: 'app-edit-phone-number',
  templateUrl: './edit-phone-number.component.html',
  styleUrls: ['./edit-phone-number.component.scss']
})
export class EditPhoneNumberComponent implements OnInit {

  @Input() phoneNumber;
  
  @Output() closeEvent = new EventEmitter();

  editPhoneNumberform: FormGroup;

  constructor(
    private fb: FormBuilder,
    private profileService: CustomerProfileService,
    private _snackbar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const phoneNumber = this.phoneNumber.slice(4);

    this.editPhoneNumberform = this.fb.group({
      phoneNumber: [phoneNumber, { validators: [Validators.required, Validators.pattern(".{9,9}")] }]
    });
  }

  model = '';

  editPhoneNumber() {
    this.model = this.editPhoneNumberform.get('phoneNumber').value;

    this.profileService.changePhoneNumber('962' + this.model).subscribe({
      next: () => { 
        this.closeForm();
        this.openSnackbar("phone number edited successfully", "ok");
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("an error occured during upload, please try again", "ok");
        }
      }
    })
  }

  closeForm() {
    this.closeEvent.emit(this.model);
  }

  openSnackbar(message, action) {
    this._snackbar.open(message, action, { duration: 3000 });
  }

}
