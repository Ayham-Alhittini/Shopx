import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReportService } from 'src/app/services/customer-product/report.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProductReport } from 'src/app/models/productReport';

@Component({
  selector: 'app-report-dialog',
  templateUrl: './report-dialog.component.html',
  styleUrls: ['./report-dialog.component.scss']
})
export class ReportDialogComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private reportService: ReportService,
    @Inject(MAT_DIALOG_DATA) _data: { id: number },
    public dialogRef: MatDialogRef<ReportDialogComponent>,
  ) {
    this.id = _data.id;
  }

  id: number;
  reportForm: FormGroup;
  reportReasons: string[] = [];
  
  ngOnInit(): void {
    this.reportService.getReportReasons().subscribe((res) => this.reportReasons = res);

    this.reportForm = this.fb.group({
      reportReason: ['', { validators: [Validators.required] }],
      reportDetails: ['', { validators: [Validators.required]}],
    });

    this.reportForm.get('reportDetails').disable();

    this.reportForm.get('reportReason').valueChanges.subscribe((res) => {
      if(res === 'Other') {
        this.reportForm.get('reportDetails').enable();
      } else {
        this.reportForm.get('reportDetails').disable();
      }
    });
  }

  reportProduct() {
    const report: ProductReport = {
      productId: this.id,
      reportReason: this.reportForm.get('reportReason').value,
      reportDetails: this.reportForm.get('reportDetails').value,
    }
    this.reportService.reportProduct(report).subscribe(() => this.dialogRef.close(true));
  }


}
