import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, Input } from '@angular/core';
import { HandsetService } from 'src/app/services/handset/handset.service';

@Component({
  selector: 'app-product-section',
  templateUrl: './product-section.component.html',
  styleUrls: ['./product-section.component.scss'],
})
export class ProductSectionComponent {

  isHandset$ = this.handsetObserver.getIsHandset();

  private _products: { productId: number, url: string }[];

  @Input() 
  set products(value){
    this._products = [ ...value ];
  }
  get products() {
    return this._products;
  }

  @Input() title;
  @Input() sectionLink;

  constructor(private handsetObserver: HandsetService) {

  }

  scrollBy(images: HTMLDivElement, value: number) {
    images.scrollBy({
      left: value,
      behavior: 'smooth'
    });
  }
}
