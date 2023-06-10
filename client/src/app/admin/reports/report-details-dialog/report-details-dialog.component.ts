import { Component, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
@Component({
  selector: 'app-report-details-dialog',
  templateUrl: './report-details-dialog.component.html',
  styleUrls: ['./report-details-dialog.component.scss']
})
export class ReportDetailsDialogComponent{
  constructor(
    @Inject(MAT_DIALOG_DATA) public details: any
 ) { }
}
