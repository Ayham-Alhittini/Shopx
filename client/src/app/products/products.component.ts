import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from '../models/Product';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ProductService } from '../services/product/product.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {

  constructor(
    private productService: ProductService,
    private _snackbar: MatSnackBar
  ) {}

  @ViewChild(MatPaginator) paginator: MatPaginator;
  
  products: Product[] = [];
  onPageProducts: Product[] = [];

  noProducts = false;
  isLoading = true;

  ngOnInit(): void {

    this.productService.getProducts(1, 50).subscribe({
      next: (products) => {
        this.products = products;
        this.onPageProducts = this.products.slice(0, 5);

        if(this.products.length <= 0) {
          this.noProducts = true;
        }
        this.isLoading = false;
      }
    });

  }

  handlePageEvent(event: PageEvent) {

    this.updateProductsList(event.pageIndex, event.pageSize, event.length);

    //scroll to the top if page is changed.
    document.querySelector('.mat-sidenav-content').scrollTop = 0;
  }
  
  onDelete(id: number) {
    ///// remove from database.
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        ///// if last product on page go to previous page.
        const isLast = this.products.length % this.paginator.pageSize === 1;
        if(isLast) {
          this.paginator.previousPage();
        }
        
        ///// remove from dom.
        this.products = this.products.filter(product => {
          return product.id !== id;
        });

        if(this.products.length === 0) {
          this.noProducts = true;
        }
        
        ///// update onPageProducts
        this.updateProductsList(this.paginator.pageIndex, this.paginator.pageSize, this.paginator.length);
        this.openSnackbar("product with id: " + id + " was deleted successfully", "ok");
      }
    });
  }

  private updateProductsList(pageIndex: number, pageSize: number, length: number) {
    const startIndex = pageIndex * pageSize;
    var endIndex = startIndex + pageSize;
    if(endIndex > length) {
      endIndex = length;
    }

    this.onPageProducts = this.products.slice(startIndex, endIndex);
  }

  openSnackbar(message: string, action: string) {
    this._snackbar.open(message, action, { duration: 3000 });
  }

}
