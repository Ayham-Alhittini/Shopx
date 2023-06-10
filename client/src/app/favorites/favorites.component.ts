import { Component, OnInit, ViewChild } from '@angular/core';
import { WishlistService } from '../services/customer-product/wishlist.service';
import { Product } from '../models/Product';
import { CartService } from '../services/customer-product/cart.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.scss']
})
export class FavoritesComponent implements OnInit {

  constructor(
    private wishlistService: WishlistService,
  ) {}

  @ViewChild(MatPaginator) paginator: MatPaginator;
  
  favoriteProducts: Product[] = [];
  onPageProducts: Product[] = [];
  loading = true;

  ngOnInit(): void {
    this.wishlistService.getWishlist().subscribe(products => {
      this.favoriteProducts = products;
      this.onPageProducts = this.favoriteProducts.slice(0, 5);
      this.loading = !this.loading;
    });
  }

  removeFromFavorites(id: number) {
    this.wishlistService.removeFromWishlist(id).subscribe(() => {
      ///// if last product on page go to previous page.
      const isLast = this.favoriteProducts.length % this.paginator.pageSize === 1;
      if(isLast) {
        this.paginator.previousPage();
      }
      
      ///// remove from dom.
      this.favoriteProducts = this.favoriteProducts.filter(product => {
        return product.id !== id;
      });
      
      ///// update onPageProducts
      this.updateProductsList(this.paginator.pageIndex, this.paginator.pageSize, this.paginator.length);
    });
  }

  handlePageEvent(event: PageEvent) {

    this.updateProductsList(event.pageIndex, event.pageSize, event.length);

    //scroll to the top if page is changed.
    document.querySelector('.mat-sidenav-content').scrollTop = 0;
  }

  private updateProductsList(pageIndex: number, pageSize: number, length: number) {
    const startIndex = pageIndex * pageSize;
    var endIndex = startIndex + pageSize;
    if(endIndex > length) {
      endIndex = length;
    }

    this.onPageProducts = this.favoriteProducts.slice(startIndex, endIndex);
  }
}
