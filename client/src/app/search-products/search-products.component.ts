import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InitProductService } from '../services/init/init-product.service';
import { FilterService } from '../services/filter/filter.service';
import { Product } from '../models/Product';
import { Category } from '../models/categories';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Observable, map } from 'rxjs';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-search-products',
  templateUrl: './search-products.component.html',
  styleUrls: ['./search-products.component.scss']
})
export class SearchProductsComponent implements OnInit {

  categories: Category[];
  category: Category;
  model;
  isGeneric: boolean = false;
  products: Product[] = [];
  colapseFilter: Observable<boolean>;
  filterClosed: boolean = true;
  loading: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  pagination;
  query: { searchContent: string; minPrice: string; maxPrice: string; model: {}; };

  constructor(
    private route: ActivatedRoute,
    private initService: InitProductService,
    private filterService: FilterService,
    private breakPoint: BreakpointObserver
  ) {
    this.colapseFilter = this.breakPoint.observe('(min-width: 700px)').pipe(map(({matches}) => matches ? true : false ));
  }

  categoryLink;

  ngOnInit(): void {
    this.route.paramMap.subscribe(paramMap => {
      this.categoryLink = paramMap['params']['category'];

      if(this.categoryLink === 'all') {
        this.isGeneric = true;
      } else {
        this.isGeneric = false;
        this.getModel(this.categoryLink);
      }
      
      this.initService.getCategories().subscribe({
        next: (categories) => {
          this.categories = categories;
          this.category = categories.find((category) => category.link === this.categoryLink);
          
          //// TO DO if isGeneric is not true and category is undefiend route to not found

          this.route.queryParamMap.subscribe((paramMap) => {
            const searchParam = paramMap['params']['searchContent'];
            let query = { searchContent: '', minPrice: '', maxPrice: '', model: {} };

            if(searchParam) {
              query = { ...query, searchContent: searchParam };
            }

            this.filterProducts(query);
          });
        }
      });
      this.query = { searchContent: '', minPrice: '', maxPrice: '', model: {} }; /// reset for paginater
    });

  }

  getModel(categoryLink) {
    this.initService.getModel(categoryLink).subscribe({
      next: (model) => {
        for(let spec of Object.keys(model)) {
          model[spec].rules = [];
        }
        this.model = model;
      }
    });
  }

  filterProducts(query: { searchContent: string; minPrice: string; maxPrice: string; model: {}; }, pageNumber='1', pageSize='5') {
    this.loading = true; /// reset loading states
    
    this.query = query; /// for use by paginator
    let link;
    if(this.paginator) {
      pageSize = this.paginator.pageSize + '';
    }
    if(this.category !== undefined) {
      link = this.category.link;
    } 
    else if(this.isGeneric) {
      this.filterService.genericFilter('', query.searchContent, query.minPrice, query.maxPrice,
        pageNumber, pageSize).subscribe(res => {
          this.handleResponse(res, pageNumber);
        });
      return; // return if generic and dont enter category
    } else return; // return if not generic nor category

    this.filterService.filterGategory(link, query.model, query.searchContent, 
      query.minPrice, query.maxPrice, pageNumber, pageSize)
      .subscribe(res => {
        this.handleResponse(res, pageNumber);
      });
  }

  handlePageEvent(event: PageEvent) {
    const pageNumber = (event.pageIndex + 1) + '';
    const pageSize = event.pageSize + '';
    this.filterProducts(this.query, pageNumber, pageSize);
  }

  private handleResponse(res, pageNumber: string) {
    this.products = res.products;
    this.pagination = res.pagination;
    this.paginator.length = this.pagination.totalItems;
    if(pageNumber === '1')
        this.paginator.pageIndex = 0; // go back to  first page

    this.loading = false; /// set loading states 
    document.querySelector('.mat-sidenav-content').scrollTop = 0; //scroll to the top if page is changed.
  }

}
