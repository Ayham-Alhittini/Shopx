import { Component , OnInit} from '@angular/core';
import { User } from '../../models/user';
import { AdminService } from 'src/app/services/admin/admin.service';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { Pagination } from 'src/app/models/pagination';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html',
  styleUrls: ['./manage-users.component.scss']
})
export class ManageUsersComponent implements OnInit {

  displayOptions = [
    'photo',
    'id',
    'userName',
    'knownAs',
    'state',
    'accountType',
    'email',
    'lastActive',
    'created',
    'operations'
  ];

  
  displayOptions2 = [
    'photo',
    'id',
    'knownAs',
    'state',
    'accountType',
    'email',
    'lastActive',
    'created',
    'operations'
  ];



  pageNumer = 1;
  pageSize = 25;
  pagination: Pagination;
  userType = 'Seller';
  userState = 'active';

  searchResult = false;
  searchContent = '';

  users: User[] = [];

  allowedToPerform = true;
  loading = true;

  constructor(private adminService: AdminService, private _snackBar: MatSnackBar){}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.users.splice(0);
    this.loading = true;
    this.searchResult = false;
    this.allowedToPerform = true;
    // console.log(this.userType)
    this.adminService.getUsers(this.userType, this.userState, this.pageNumer, this.pageSize).subscribe({
      next: res => {
        this.users = res.result;
        this.pagination = res.pagination;
        this.loading = false;
        // console.log(this.users); 
      }
    });
  }

  performSearch() {
    this.searchResult = true;
    this.adminService.getUser(this.searchContent).subscribe({
      next: res => {
        this.users = [res];
        if (res.accountState != 'active' && res.accountState != 'banned')
        {
          this.allowedToPerform = false;
        }
        else 
        {
          this.allowedToPerform = true;
        }

      },
      error: err => {
        this._snackBar.open('User Not Exit', 'x', {
          duration: 3000
        });
      }
    });
    this.searchContent = '';
  }


  handlePageEvent(event: PageEvent) {

    this.pageNumer = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    
    this.getUsers();

    //scroll to the top if page is changed.
    document.querySelector('.mat-sidenav-content').scrollTop = 0;
  }

  blockToggle(userId: string) {
    let command = this.userState === 'active';
    
    this.adminService.blockUser(userId, command).subscribe({
      next: res => {
        this._snackBar.open(res['response'], 'x', {
          duration: 3000
        });
        this.users = this.users.filter((item) => item.id !== userId);
      }
    });
  }
}
