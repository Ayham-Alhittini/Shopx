import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-review-customer',
  templateUrl: './review-customer.component.html',
  styleUrls: ['./review-customer.component.scss']
})
export class ReviewCustomerComponent {

  @Input() review;
  
}
