import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EditComponent } from '../edit/edit.component';
import { ProfileService } from '../services/profile/profile.service';
import { SellerProfileService } from '../services/profile/seller-profile.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-seller-profile',
  templateUrl: './seller-profile.component.html',
  styleUrls: ['./seller-profile.component.scss']
})
export class SellerProfileComponent implements OnInit {

  constructor(
    private profileService: ProfileService,
    public dialog: MatDialog,
    private sellerProfileService: SellerProfileService,
    private _snackbar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.sellerProfileService.getSellerProfile().subscribe({
      next: (profile: any) => {
        this.profile = { ...profile };
        this.photoUrl = profile.backgroundPhoto.url;

        this.imageIsLoaded = true;
      }
    })
  }

  profile;
  editProfile = false;
  changePassword = false;
  imageIsLoaded = false;
  photoUrl = '';

  imageEditDisabled = false;

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
  dialogDescreption = "This image will be your shop's image, that will help customers identify you.";
  dialogUrl = "";

  openDialog() {
    const dialogRef = this.dialog.open(EditComponent, {
      disableClose: true,
      data: {
        title: this.dialogTitle,
        description: this.dialogDescreption,
        initialUrl: this.dialogUrl
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
    if(Object.keys(model).length)
      this.profile = { ...model };
    this.toggleEditForm();
  }

  openSnackbar(message, action) {
    this._snackbar.open(message, action, { duration: 3000 });
  }
}

