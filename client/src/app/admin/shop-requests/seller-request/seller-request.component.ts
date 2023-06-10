import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ShopRequest } from 'src/app/models/ShopRequest';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-seller-request',
  templateUrl: './seller-request.component.html',
  styleUrls: ['./seller-request.component.scss']
})
export class SellerRequestComponent{

  constructor (
    private admin: AdminService,
    private _snackBar: MatSnackBar){}

  @Input()request :ShopRequest;


  onAnswer() {
    this.admin.onRequestAnswer.emit(this.request);
  }

  onAccept(){
    this.openSnackBar(this.request.userName + ', accepted successfuly', 'ok');
    this.onAnswer();

    this.admin.acceptShop(this.request.userName).subscribe();
  }

  onReject() {
    const reason = prompt('Reason for rejection');
    if (reason)
    {
      this.openSnackBar(this.request.userName + ', rejected successfuly', 'ok');
      this.onAnswer();

      this.admin.rejectShop(this.request.userName, reason).subscribe();
    }
    
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 3000
    });
  }
}
