import { Component, OnInit } from '@angular/core';
import { FilterService } from '../services/filter/filter.service';
import { InitProductService } from '../services/init/init-product.service';
import { Category } from '../models/categories';
import { Product } from '../models/Product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  genericProducts: { productId: number, url: string }[] = [];
  categoriesProducts: { label: string, link: string, products: { productId: number, url: string }[] }[] = [];

  categories: Category[];


  constructor(
    private filterService: FilterService,
    private initService: InitProductService
  ) {}
  
  ngOnInit(): void {
    this.filterService.genericFilter('', '', '', '', '1', '10').subscribe({
      next: (res) => {
        this.genericProducts = this.getProducts(res.products);
      }
    });

    this.initService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;

        for(let category of categories) {
          this.filterService.genericFilter(category.label, '', '', '', '1', '10').subscribe({
            next: (res) => {
              const categoryProducts = { label: category.label, link: category.link, products: this.getProducts(res.products) }
              this.categoriesProducts = [ ...this.categoriesProducts, categoryProducts ];
            }
          });
        }
      }
    });
  }

  private getProducts(products: Product[]) {
    let productsArr: { productId: number, url: string }[] = [];

    for(let i = 0; i < 10 && i < products.length; i++) {
      let url;

      if(products[i]?.productPhotos.length > 0) {
        url = products[i].productPhotos[0].url;
      } else {
        url = "/assets/no-image.png";
      }
      productsArr = [ ...productsArr, { productId: products[i].id, url: url } ];
    }

    return productsArr;
  }

 

}
