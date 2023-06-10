import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { CartService } from 'src/app/services/customer-product/cart.service';

@Component({
  selector: 'app-favorite-product',
  templateUrl: './favorite-product.component.html',
  styleUrls: ['./favorite-product.component.scss']
})
export class FavoriteProductComponent implements OnInit {

  @Output() removeFromFavoritesEvent = new EventEmitter<number>();

  @Input() product: Product;

  onWishlist: boolean;
  onCart: boolean;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.onCart = this.product.onCart;
    this.onWishlist = this.product.onWishlist;
  }

  toggleFavorites() {
    this.removeFromFavoritesEvent.emit(this.product.id);
  }

  toggleShoppingCart() {
    if(!this.onCart) {
      this.cartService.addToCart({ productId: this.product.id, quantity: 1 }).subscribe(() => this.onCart = !this.onCart);
    } else {
      this.cartService.removeFromCart(this.product.id).subscribe(() => this.onCart = !this.onCart);
    }
  }
}
