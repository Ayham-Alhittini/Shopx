import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Product } from 'src/app/models/Product';
import { Pagination } from 'src/app/models/pagination';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-manage-products',
  templateUrl: './manage-products.component.html',
  styleUrls: ['./manage-products.component.scss']
})
export class ManageProductsComponent implements OnInit{

  searchProductId = null;


  displayOptions = ['photo', 'id', 'title', 'category', 'subCategory', 'state', 'sellerName', 'price', 'quantity', 'reportCount', 'operations'];
  pageNumber = 1;
  pageSize = 25;
  pagination: Pagination;
  products: Product[] = [];

  productState = 'active';
  shops : string[] = [];
  selectedShop = '';
  searchResult = false;

  operationRequest = false;
  constructor(private adminService: AdminService,
    private router: Router,
    private _snackBar: MatSnackBar){}

  ngOnInit() {

    this.getProducts();

    this.adminService.getShopsSearch().subscribe({
      next: res => {
        this.shops = res;
      }
    });

  }

  Reset() {
    this.productState = 'active';
    this.selectedShop = '';
    this.searchProductId = null;
    this.getProducts();
  }

  getProducts() {
    this.searchResult = false;
    this.adminService.getProducts(this.pageNumber, this.pageSize, this.productState, this.selectedShop).subscribe({
      next : res => {
        this.products = res.result;
        this.pagination = res.pagination;
  
      }
    });
  }
  
  onRowClick(id : number) {
    if (this.operationRequest)
    {
      this.operationRequest = false;
      return;
    }
    window.open('home/products/' + id, '_blank');
  }

  blockToggle(product: Product) {
    this.operationRequest = true;
    
    if (product.state === 'banned') {///unblock
      this.adminService.unBlockProduct(product.id).subscribe({
        next: res => {
          this._snackBar.open(res['response'], 'x', {
            duration: 3000
          });
        }
      });
    } else {
      ///ask for reason for block
      const reason = prompt('Reason for block');
      if (!reason) {
        return;
      }
      this.adminService.blockProduct(product.id, reason).subscribe({
        next: res => {
          this._snackBar.open(res['response'], 'x', {
            duration: 3000
          });
        }
      });
    }
    
    this.products = this.products.filter(p => p.id !== product.id);
  }

  showReports(productId: number) {
    this.operationRequest = true;
    this.router.navigateByUrl("admin/reports/" + productId);
  }

  onSearchSubmit() {
    this.searchResult = true;
    // console.log(this.searchProductId);
    if (!this.searchProductId)return;
    this.adminService.getProduct(this.searchProductId).subscribe({
      next : res => {
        this.clear();
        if (res)
        {
          this.products.push(res);
        }
      }
    });
  }

  clear() {
    this.products = [];
  }

  getLabelState(state: string) {
    return state === 'banned' ? 'Unblock' : 'Block';
  }

  handlePageEvent(event: PageEvent) {

    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    
    this.getProducts();

    //scroll to the top if page is changed.
    document.querySelector('.mat-sidenav-content').scrollTop = 0;
  }
}

