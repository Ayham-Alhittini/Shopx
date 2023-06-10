import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Report } from 'src/app/models/report';
import { AdminService } from 'src/app/services/admin/admin.service';
import { ReportDetailsDialogComponent } from './report-details-dialog/report-details-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit{

  displayedColumnsUnWatcd = ['photo','id','customerId','knownAs','reportReason','sendDate','reportDetails','markRead'];
  displayedColumnsWatcd = ['photo','id','customerId','knownAs','reportReason','sendDate','reportDetails','watchDate'];

  productState = '';

  panelOpenState = false;
  loading = false;
  
  step = 1;
  watchedReport: Report[]= [];
  unWatchedReport: Report[] = [];
  
  reports: Report[] = [];
  
  productId: number = null;

  constructor(private route: ActivatedRoute,
    private adminService: AdminService,
    private dialog: MatDialog,
    private _snackBar: MatSnackBar){}

  ngOnInit(): void {

    //load product id
    this.route.params.subscribe(params => {
      this.productId = params['productId'];
    });

    ///load reports
    if (!this.productId)return;

    this.loadReports();

    this.adminService.getProduct(this.productId).subscribe({
      next: res => {
        this.productState = res.state;
      }
    });
  }


  toggleBlock() {
    if (this.productState === '')return;

    this.loading = true;

    if (this.productState === 'banned') {
      this.adminService.unBlockProduct(this.productId).subscribe({
        next: res => {

          this._snackBar.open(res['response'], 'x', {
            duration: 3000
          });
          this.productState = 'active';
          
          this.loading = false;
        }
      });
    } else {
      const reason = prompt('Enter Block Reason');
      if (reason != null)
      {
        this.adminService.blockProduct(this.productId, reason).subscribe({
          next: res => {
            this._snackBar.open(res['response'], 'x', {
              duration: 3000
            });
            this.productState = 'banned';
            this.loading = false;
          }
        });
      }
      
    }
    
  }

  loadReports() {
    if (this.productId)
    {
      this.adminService.getReports(this.productId).subscribe({
        next: res => {
          this.reports = res;
          
          this.unWatchedReport = this.reports.filter(r => r.watchDate === null);
          this.watchedReport = this.reports.filter(r => r.watchDate !== null);
          
          // console.log(this.unWatchedReport);
        }
      });
    }
  }

  openDialog(reportDetails: string) {
    this.dialog.open(ReportDetailsDialogComponent,{
      data : reportDetails
    });
  }

  reportRead(reportId) {
    if (!reportId)return;

    this.adminService.watchReport(reportId).subscribe({
      next: () => {
        this.loadReports();
      }
    });
  }
}