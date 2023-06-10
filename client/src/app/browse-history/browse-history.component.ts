import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Product } from '../models/Product';
import { BrowseHistoryService } from '../services/customer-product/browse-history.service';

@Component({
  selector: 'app-browse-history',
  templateUrl: './browse-history.component.html',
  styleUrls: ['./browse-history.component.scss']
})
export class BrowseHistoryComponent implements OnInit {

  constructor(private historyService: BrowseHistoryService) {}

  @ViewChild(MatPaginator) paginator: MatPaginator;
  
  viewedProducts: Product[] = [];
  onPageProducts: Product[] = [];
  loading = true;

  ngOnInit(): void {
    this.historyService.getHistory().subscribe(products => {
      this.viewedProducts = products;
      this.onPageProducts = this.viewedProducts.slice(0, 5);
      this.loading = !this.loading;
    });
  }

  removeFromHistory(id: number) {
    this.historyService.removeFromHistory(id).subscribe(() => {
      ///// if last product on page go to previous page.
      const isLast = this.viewedProducts.length % this.paginator.pageSize === 1;
      if(isLast) {
        this.paginator.previousPage();
      }
      
      ///// remove from dom.
      this.viewedProducts = this.viewedProducts.filter(product => {
        return product.id !== id;
      });
      
      ///// update onPageProducts
      this.updateProductsList(this.paginator.pageIndex, this.paginator.pageSize, this.paginator.length);
    });
  }

  deleteAll() {
    this.historyService.deleteAll().subscribe(() => {
      this.viewedProducts = [];
      this.updateProductsList(this.paginator.pageIndex, this.paginator.pageSize, this.paginator.length);
    })
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

    this.onPageProducts = this.viewedProducts.slice(startIndex, endIndex);
  }
}
