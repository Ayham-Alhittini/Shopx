import { Component, OnInit } from '@angular/core';
import { Category } from '../models/categories';
import { InitProductService } from '../services/init/init-product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {

  constructor(
    private initService: InitProductService
  ) {}
  
  categories: Category[]; 
  
  ngOnInit(): void {
    this.initService.getCategories().subscribe({
      next: (categories) => this.categories = categories
    })
  }
  
}
