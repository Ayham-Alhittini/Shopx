import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Product } from 'src/app/models/Product';
import { AuthService } from 'src/app/services/auth/auth.service';
import { CartService } from 'src/app/services/customer-product/cart.service';
import { WishlistService } from 'src/app/services/customer-product/wishlist.service';
import { GuestService } from 'src/app/services/guest.service';

@Component({
  selector: 'app-cart-product',
  templateUrl: './cart-product.component.html',
  styleUrls: ['./cart-product.component.scss']
})
export class CartProductComponent implements OnInit {

  @Output() removeFromCartEvent = new EventEmitter<number>();
  @Output() priceChangeEvent = new EventEmitter<number>();

  @Input() product: { product: Product, quantity: number };

  onWishlist: boolean;
  onCart: boolean;
  quantity: number;
  productQuantity: number;

  constructor(
    private wishlistService: WishlistService,
    private cartService: CartService,
    private _snackBar: MatSnackBar,
    public authService: AuthService,
    private guestService: GuestService
  ) {}

  ngOnInit(): void {
    this.onCart = this.product.product.onCart;
    this.onWishlist = this.product.product.onWishlist;
    this.quantity = this.product.quantity;
    this.productQuantity = this.product.product.quantity;
  }

  toggleFavorites() {
    if(!this.onWishlist) {
      this.wishlistService.addToWishlist(this.product.product.id).subscribe(() => this.onWishlist = !this.onWishlist);
    } else {
      this.wishlistService.removeFromWishlist(this.product.product.id).subscribe(() => this.onWishlist = !this.onWishlist);
    }
  }

  toggleShoppingCart() {
    if (this.authService.loadedUser)
      this.removeFromCartEvent.emit(this.product.product.id);
    else
      this.guestService.removeFromCart(this.product.product.id);
  }

  increaseQuantity() {
    if(this.quantity + 1 > this.productQuantity) {
      this._snackBar.open("there are only " + this.productQuantity + " of this product.", "ok", { duration: 2000 });
      return;
    }
    if (this.authService.loadedUser) {
      this.cartService.addToCart({ productId: this.product.product.id, quantity: this.quantity + 1 }).subscribe(() => {
        this.quantity = this.quantity + 1;
        this.cartService.cartCount++;
        this.priceChangeEvent.emit(this.product.product.price);
      });
    } else {
      this.quantity++;
      this.guestService.increaseQuantity(this.product);
    }
  }

  reduceQuantity() {
    if(this.quantity === 1) {
      this.toggleShoppingCart();
      return;
    }
    if (this.authService.loadedUser) {
      this.cartService.addToCart({ productId: this.product.product.id, quantity: this.quantity - 1 }).subscribe(() => {
        this.quantity = this.quantity - 1;
        this.cartService.cartCount--;
        this.priceChangeEvent.emit(-1 * this.product.product.price);
      });
    } else {
      this.quantity--;
      this.guestService.reduceQuantity(this.product);
    }
  }
}
