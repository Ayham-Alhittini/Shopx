import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../services/profile/profile.service';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../services/auth/auth.service';
import { EditComponent } from '../edit/edit.component';
import { CustomerProfileService } from '../services/profile/customer-profile.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  imageIsLoaded = false;
  changePassword = false;
  editProfile = false;
  profile;
  photoUrl = '';
  phoneNumber = '';

  imageEditDisabled = false;

  constructor(
    private profileService: ProfileService,
    private customerProfileService: CustomerProfileService,
    public dialog: MatDialog,
    private authService: AuthService,
    private _snackbar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const user = this.authService.loadedUser;
    this.profile = user;

    if(user.backgroundPhotoUrl) {
      this.photoUrl = user.backgroundPhotoUrl;
    }
    this.imageIsLoaded = true;

    this.customerProfileService.getPhoneNumber().subscribe(num => this.phoneNumber = num.phoneNumber);

  }


  editImage(result) {
    this.imageEditDisabled = true;

    const file = result.file;

    if(!file) { return };

    const formData = new FormData();
    formData.append('file', file);

    ////api call
    this.profileService.changeImage(formData).subscribe({
      next: (img) => {
        this.photoUrl = img.url; 
        this.openSnackbar("image edited", "ok");
        this.imageIsLoaded = true;
        this.imageEditDisabled = false;
      },
      error: (error) => {
        if(error && error.error && !error.error.message ) {
          this.openSnackbar(error.error, 'ok');
        } else if(error && error.message) {
          this.openSnackbar("an error occured during upload, please try again", "ok");
        }
      }
    });
  }

  dialogTitle = "Add an Image";
  dialogDescreption = "This image will be your profile's image.";
  dialogUrl = this.photoUrl;

  openDialog() {
    const dialogRef = this.dialog.open(EditComponent, {
      disableClose: true,
      data: {
        title: this.dialogTitle,
        description: this.dialogDescreption,
        initialUrl: this.photoUrl
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.editImage(result);
    });
  }

  toggleEditForm() {
    this.editProfile = !this.editProfile;
  }

  closeEditForm(model) {
    if(model)
      this.phoneNumber = model;
    this.toggleEditForm();
  }

  openSnackbar(message, action) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
}
