import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SellerProfileService } from 'src/app/services/profile/seller-profile.service';

@Component({
  selector: 'app-seller-edit-profile',
  templateUrl: './seller-edit-profile.component.html',
  styleUrls: ['./seller-edit-profile.component.scss']
})
export class SellerEditProfileComponent {

  _profile;
  @Input() 
  set profile(value) {
    this._profile = { ...value };
  }
  get profile() {
    return this._profile;
  }

  @Output() closeEvent = new EventEmitter();

  editProfileform: FormGroup;

  cities = [
    "Amman",
    "Irbid",
    "Zarqa",
    "Salt",
    "Madaba",
    "Aqaba",
    "Mafraq",
    "Jerash",
    "Al Karak",
    "Ajloun",
    "Ma'an",
    "Ramtha",
    "Tafila",
    "Dead Sea"
  ];
  
  constructor(
    private fb: FormBuilder,
    private sellerProfileService: SellerProfileService,
    private _snackbar: MatSnackBar
  ) { }

  ngOnInit(): void {
    const phoneNumber = this.profile.phoneNumber.slice(4);
    const city = this.profile.shopCity;

    this.editProfileform = this.fb.group({
      phoneNumber: [phoneNumber, { validators: [Validators.required, Validators.pattern(".{9,9}")] }],
      taxNumber: [this.profile.taxNumber, { validators: [Validators.required] }],
      shopCity: [city, { validators: [Validators.required] }],
      shopDescription: [this.profile.shopDescription, { validators: [Validators.required] }],
      bankName: [this.profile.bankName, { validators: [Validators.required] }],
      bankAccountNumber: [this.profile.bankAccountNumber, { validators: [Validators.required] }],
    });

    this.editProfileform.get('shopCity').setValue(city);
  }

  model = {};
  disableSubmit = false;

  editProfile() {
    this.disableSubmit = true;

    this.model = { ...this.editProfileform.getRawValue(),
      bankAccounNumber: '' + this.editProfileform.get('bankAccountNumber').value,
      phoneNumber: '962' + this.editProfileform.get('phoneNumber').value,
      taxNumber: '' + this.editProfileform.get('taxNumber').value, 
    };

    this.sellerProfileService.editSellerProfile(this.model).subscribe({
      next: () => { 
        this.closeForm();
        this.openSnackBar("profile edited successfully", "ok");
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackBar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackBar("an error occured, please try again", "ok");
        }
        this.disableSubmit = false;
      }
    })
  }

  openSnackBar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }

  closeForm() {
    this.closeEvent.emit(this.model);
  }

}
