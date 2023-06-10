import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProductService } from 'src/app/services/product/product.service';

@Component({
  selector: 'app-discount-dialog',
  templateUrl: './discount-dialog.component.html',
  styleUrls: ['./discount-dialog.component.scss']
})
export class DiscountDialogComponent {

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    @Inject(MAT_DIALOG_DATA) _data: { id: number },
    public dialogRef: MatDialogRef<DiscountDialogComponent>,
  ) {
    this.id = _data.id;
  }

  id: number;
  discountForm: FormGroup;
  reportReasons: string[] = [];
  
  ngOnInit(): void {
    this.discountForm = this.fb.group({
      discountRate: ['', { validators: [Validators.required, Validators.max(100), Validators.min(0)] }],
    });
  }

  setDiscount() {
    const rate = this.discountForm.get('discountRate').value;
    this.productService.setDiscountRate(this.id, rate).subscribe(() => this.dialogRef.close(rate));
  }
  
}
