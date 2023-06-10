import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/app/models/Product';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {

  @Output() deleteEvent = new EventEmitter<number>();

  @Input() product: Product;

  deleteProduct() {
    this.deleteEvent.emit(this.product.id);
  }

}
