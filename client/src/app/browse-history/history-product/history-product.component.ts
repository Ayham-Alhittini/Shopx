import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { CartService } from 'src/app/services/customer-product/cart.service';
import { WishlistService } from 'src/app/services/customer-product/wishlist.service';

@Component({
  selector: 'app-history-product',
  templateUrl: './history-product.component.html',
  styleUrls: ['./history-product.component.scss']
})
export class HistoryProductComponent {

  @Output() removeFromHistoryEvent = new EventEmitter<number>();

  @Input() product: Product;

  onWishlist: boolean;
  onCart: boolean;

  constructor(
    private cartService: CartService,
    private wishlistService: WishlistService
  ) {}

  ngOnInit(): void {
    this.onCart = this.product.onCart;
    this.onWishlist = this.product.onWishlist;
  }

  toggleFavorites() {
    if(!this.onWishlist) {
      this.wishlistService.addToWishlist(this.product.id).subscribe(() => this.onWishlist = !this.onWishlist);
    } else {
      this.wishlistService.removeFromWishlist(this.product.id).subscribe(() => this.onWishlist = !this.onWishlist);
    }
  }

  toggleShoppingCart() {
    if(!this.onCart) {
      this.cartService.addToCart({ productId: this.product.id, quantity: 1 }).subscribe(() => this.onCart = !this.onCart);
    } else {
      this.cartService.removeFromCart(this.product.id).subscribe(() => this.onCart = !this.onCart);
    }
  }

  removeFromHistory() {
    this.removeFromHistoryEvent.emit(this.product.id);
  }
}
