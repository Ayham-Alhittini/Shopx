import { Component, Input, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { CartService } from 'src/app/services/customer-product/cart.service';
import { WishlistService } from 'src/app/services/customer-product/wishlist.service';

@Component({
  selector: 'app-searched-product-card',
  templateUrl: './searched-product-card.component.html',
  styleUrls: ['./searched-product-card.component.scss']
})
export class SearchedProductCardComponent implements OnInit {

  @Input() product: Product;

  onWishlist: boolean;
  onCart: boolean;

  constructor(
    private wishlistService: WishlistService,
    private cartService: CartService
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
}
